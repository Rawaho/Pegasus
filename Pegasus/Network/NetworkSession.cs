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
using Pegasus.Network.Packet.Object;
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
        private readonly ConcurrentQueue<BasePacket> incomingPackets = new ConcurrentQueue<BasePacket>();
        private readonly Queue<ServerPacket> outgoingPackets = new Queue<ServerPacket>();

        // current packet being processed
        private FragmentedBuffer onDeck;

        private double timeSinceLastPing;

        /// <summary>
        /// Enqueue <see cref="IWritable"/> to be sent to the client.
        /// </summary>
        public void EnqueueMessage(IWritable message)
        {
            if (!PacketManager.GetRawOpcode(message, out ServerRawOpcode opcode))
            {
                log.Warn("Failed to send raw packet with no attribute!");
                return;
            }

            var packet = new ServerPacket(opcode, message);
            outgoingPackets.Enqueue(packet);

            log.Trace($"Sent raw packet {opcode}(0x{opcode:X}).");
        }

        /// <summary>
        /// Enqueue <see cref="NetworkObject"/> to be sent to the client.
        /// </summary>
        public void EnqueueMessage(ObjectOpcode opcode, NetworkObject message)
        {
            var packet = new NetworkObject();
            packet.AddField(0, NetworkObjectField.CreateIntField((int)opcode));
            packet.AddField(1, NetworkObjectField.CreateObjectField(message));
            EnqueueMessage(packet);
        }

        /// <summary>
        /// Enqueue <see cref="NetworkObject"/> to be sent to the client.
        /// </summary>
        public void EnqueueMessage(NetworkObject message)
        {
            var packet = new ServerPacket(message);
            outgoingPackets.Enqueue(packet);

            ObjectOpcode opcode = (ObjectOpcode)NetworkObjectField.ReadIntField(message.GetField(0));
            log.Trace($"Sent object packet {opcode}(0x{opcode:X}).");
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
            catch (Exception e)
            {
                log.Error(e);
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

                byte[] data = new byte[length];
                Buffer.BlockCopy(buffer, 0, data, 0, length);

                ProcessPayload(data);
                BeginReceive();
            }
            catch (SocketException)
            {
                Disconnect();
            }
            catch (Exception e)
            {
                log.Error(e);
                BeginReceive();
            }
        }

        public virtual void Disconnect()
        {
            State = SessionState.ShuttingDown;
            outgoingPackets.Clear();
            incomingPackets.Clear();
        }

        private void ProcessPayload(byte[] data)
        {
            using (var stream = new MemoryStream(data))
            using (var reader = new BinaryReader(stream))
            {
                while (stream.Remaining() != 0L)
                {
                    // no packet on deck waiting for additional information, new data will be part of a new packet
                    if (onDeck == null)
                    {
                        uint size = reader.ReadUInt32();
                        onDeck = new FragmentedBuffer(size);
                    }

                    onDeck.Populate(reader);
                    if (onDeck.IsComplete)
                    {
                        BasePacket packet;
                        if (receivedFirstPacket)
                            packet = new ClientPacket(onDeck.Data);
                        else
                        {
                            // first packet doesn't follow the standard structure
                            packet = new ClientFirstPacket(onDeck.Data);
                            receivedFirstPacket = true;
                        }

                        incomingPackets.Enqueue(packet);
                        onDeck = null;
                    }
                }
            }
        }

        private void ProcessPacket(BasePacket packet)
        {
            // first packet doesn't follow the standard structure
            if (packet is ClientFirstPacket)
            {
                ProcessFirstPacket(packet);
                return;
            }

            if (packet.Size == 0u)
            {
                ProcessPingPacket();
                return;
            }

            byte[] payload = packet.Data;
            if ((packet.Flags & PacketFlag.Encrypted) != 0)
                payload = decryptor.Decrypt(payload);

            if ((packet.Flags & PacketFlag.Raw) != 0)
                ProcessRawPacket(payload);
            else
                ProcessObjectPacket(payload);
        }

        private void ProcessFirstPacket(BasePacket packet)
        {
            if (packet.Size != 4u)
                throw new InvalidDataException();

            byte[] check = new byte[4];
            Buffer.BlockCopy(packet.Data, 0, check, 0, check.Length);

            // client sends a static payload encrypted
            if (!decryptor.Decrypt(check).SequenceEqual(new byte[] {0xFF, 0xFF, 0xFF, 0xFF, 0x00}))
                throw new InvalidDataException();
        }

        private void ProcessPingPacket()
        {
            timeSinceLastPing = 0d;
        }

        private void ProcessRawPacket(byte[] payload)
        {
            using (var stream = new MemoryStream(payload))
            using (var reader = new BinaryReader(stream))
            {
                ClientRawOpcode opcode = (ClientRawOpcode)reader.ReadPackedUInt32();

                IReadable packet = PacketManager.CreateRawMessage(opcode);
                if (packet == null)
                    return;

                log.Trace($"Received raw packet {opcode}(0x{opcode:X}).");

                packet.Read(reader);
                if (stream.Remaining() > 0)
                    log.Warn($"Failed to read entire contents of packet {opcode}(0x{opcode:X}).");

                PacketManager.InvokeRawMessageHandler(this, opcode, packet);
            }
        }

        private void ProcessObjectPacket(byte[] payload)
        {
            NetworkObject networkObject = NetworkObject.UnPack(payload);

            ObjectOpcode opcode = (ObjectOpcode)NetworkObjectField.ReadIntField(networkObject.GetField(0));
            log.Trace($"Received object packet {opcode}(0x{opcode:X}).");

            if (opcode == ObjectOpcode.Authenticate)
                PacketManager.InvokeObjectMessageHandler(this, opcode, networkObject);
            else
                PacketManager.InvokeObjectMessageHandler(this, opcode, networkObject.GetField(1).ReadObject());
        }

        private void FlushPacket(ServerPacket serverPacket)
        {
            using (var stream = new MemoryStream())
            using (var writer = new BinaryWriter(stream))
            {
                writer.Write(serverPacket.Size);
                writer.WritePackedUInt32((uint)serverPacket.Flags);
                writer.Write(serverPacket.Data);

                socket.Send(stream.ToArray());
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

            while (incomingPackets.TryDequeue(out BasePacket packet))
            {
                try
                {
                    ProcessPacket(packet);
                }
                catch (Exception e)
                {
                    log.Error(e);
                    Disconnect();
                }
            }

            while (outgoingPackets.TryDequeue(out ServerPacket packet))
            {
                try
                {
                    FlushPacket(packet);
                }
                catch (SocketException)
                {
                    Disconnect();
                }
                catch (Exception e)
                {
                    log.Error(e);
                }
            }
        }
    }
}
