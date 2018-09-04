using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using NLog;
using Pegasus.Cryptography;
using Pegasus.Network.Packet;
using Pegasus.Network.Packet.Raw;

namespace Pegasus.Network
{
    public abstract class NetworkSession
    {
        private static readonly Logger log = LogManager.GetCurrentClassLogger();

        public SessionState State { get; protected set; }
        public IPEndPoint Remote => (IPEndPoint)socket.RemoteEndPoint;

        private Socket socket;
        private readonly byte[] buffer = new byte[0x1000];
        private bool receivedFirstPacket;

        private readonly PacketEncryptor encryptor = new PacketEncryptor();
        private readonly PacketDecryptor decryptor = new PacketDecryptor();
        private readonly ConcurrentQueue<ClientPacket> incomingPackets = new ConcurrentQueue<ClientPacket>();
        private readonly Queue<ServerPacket> outgoingPackets = new Queue<ServerPacket>();

        // current packet being processed
        private ClientPacket currentPacket;

        private double timeSinceLastPing = 0d;

        /// <summary>
        /// Enqueue <see cref="ServerPacket"/> to be sent to client.
        /// </summary>
        public void EnqueuePacket(ServerPacket serverPacket)
        {
            outgoingPackets.Enqueue(serverPacket);
        }

        public void Accept(Socket newSocket)
        {
            socket = newSocket;
        }

        public void BeginReceive()
        {
            try
            {
                socket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, ReceiveCallback, null);
            }
            catch (SocketException)
            {
                Disconnect();
            }
            catch (Exception exception)
            {
                log.Error(exception);
            }
        }

        private void ReceiveCallback(IAsyncResult result)
        {
            try
            {
                int length = socket.EndReceive(result);
                if (length == 0)
                {
                    Disconnect();
                    return;
                }

                byte[] messageBuffer = new byte[length];
                Buffer.BlockCopy(buffer, 0, messageBuffer, 0, length);

                ProcessPayload(messageBuffer);
                BeginReceive();
            }
            catch (SocketException)
            {
                Disconnect();
            }
            catch (Exception exception)
            {
                log.Error(exception);
                BeginReceive();
            }
        }

        public virtual void Disconnect()
        {
            State = SessionState.ShuttingDown;
            outgoingPackets.Clear();
            incomingPackets.Clear();
        }

        /// <summary>
        /// 
        /// </summary>
        private void ProcessPayload(byte[] payload)
        {
            for (int payloadOffset = 0; payloadOffset < payload.Length; )
            {
                if (currentPacket == null)
                {
                    if (payload.Length - payloadOffset < 4)
                        throw new InvalidDataException();

                    currentPacket = new ClientPacket(BitConverter.ToInt32(payload, payloadOffset) + sizeof(int));
                } 
                    
                int payloadLength = Math.Min(currentPacket.Remaining, payload.Length - payloadOffset);
                currentPacket.AddFragment(payload, payloadOffset, payloadLength);
                payloadOffset += payloadLength;

                // enqueue packet for handling if all fragments are accounted for
                if (!currentPacket.IsFragmented)
                {
                    incomingPackets.Enqueue(currentPacket);
                    currentPacket = null;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void ProcessPacket(byte[] payload)
        {
            // first packet doesn't follow the standard structure
            if (!receivedFirstPacket)
            {
                ProcessFirstPacket(payload);
                return;
            }

            if (payload.Length == sizeof(int))
            {
                ProcessPingPacket();
                return;
            }

            using (var stream = new MemoryStream(payload))
            {
                using (var reader = new BinaryReader(stream))
                {
                    reader.ReadUInt32();
                    PacketFlag flags = (PacketFlag)reader.ReadPackedUInt32();

                    byte[] packetPayload;
                    if ((flags & PacketFlag.Encrypted) != 0)
                        packetPayload = decryptor.Decrypt(payload);
                    else
                        packetPayload = reader.ReadBytes((int)(stream.Length - stream.Position));

                    if ((flags & PacketFlag.Raw) != 0)
                        ProcessRawPacket(packetPayload);
                    else
                        ProcessObjectPacket(packetPayload);
                }
            }
        }

        private void ProcessFirstPacket(byte[] payload)
        {
            if (payload.Length != 8)
                throw new InvalidDataException();

            byte[] check = new byte[4];
            Buffer.BlockCopy(payload, 4, check, 0, check.Length);

            // client sends a static payload encrypted
            if (!decryptor.Decrypt(check).SequenceEqual(new byte[] { 0xFF, 0xFF, 0xFF, 0xFF, 0x00 }))
                throw new InvalidDataException();

            receivedFirstPacket = true;
        }

        private void ProcessPingPacket()
        {
            timeSinceLastPing = 0d;
        }

        /// <summary>
        /// 
        /// </summary>
        private void ProcessRawPacket(byte[] payload)
        {
            using (var stream = new MemoryStream(payload))
            {
                using (var reader = new BinaryReader(stream))
                {
                    ClientRawOpcode opcode = (ClientRawOpcode)reader.ReadPackedUInt32();

                    ClientRawPacket packet = PacketManager.CreateRawPacket(opcode);
                    if (packet != null)
                    {
                        packet.Read(reader);
                        PacketManager.InvokeRawPacketHandler(this, opcode, packet);
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void ProcessObjectPacket(byte[] payload)
        {
            try
            {
                NetworkObject networkObject = NetworkObject.UnPack(payload);

                ObjectOpcode opcode = (ObjectOpcode)NetworkObjectField.ReadIntField(networkObject.GetField(0));
                if (opcode == ObjectOpcode.Authenticate)
                {
                    PacketManager.InvokeObjectPacketHandler(this, opcode, networkObject);
                }
                else
                {
                    PacketManager.InvokeObjectPacketHandler(this, opcode, networkObject.GetField(1).ReadObject());
                }
            }
            catch (OutOfMemoryException exception)
            {
                log.Fatal(exception);
                log.Fatal($"Payload Length: {payload.Length}");
                throw;
            }
        }

        private void FlushOutgoingPackets()
        {
            try
            {
                while (outgoingPackets.Count > 0)
                {
                    ServerPacket outgoingPacket = outgoingPackets.Dequeue();
                    socket.Send(outgoingPacket.Construct(encryptor));
                }
            }
            catch (Exception)
            {
                Disconnect();
            }
        }

        public virtual void Update(double lastTick)
        {
            timeSinceLastPing += lastTick;
            if (timeSinceLastPing > 30d)
            {
                Disconnect();
                return;
            }

            while (!incomingPackets.IsEmpty)
            {
                incomingPackets.TryDequeue(out ClientPacket incomingPacket);

                try
                {
                    ProcessPacket(incomingPacket.GetData());
                }
                catch (Exception exception)
                {
                    log.Error(exception);
                }   
            }

            FlushOutgoingPackets();
        }
    }
}
