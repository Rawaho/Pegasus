using System.IO;
using Pegasus.Cryptography;

namespace Pegasus.Network.Packet
{
    public class ServerObjectPacket : ServerPacket
    {
        protected ObjectOpcode opcode;
        protected NetworkObject networkObject;

        public ServerObjectPacket(ObjectOpcode opcode, NetworkObject networkObject, bool encrypt = true)
        {
            this.opcode        = opcode;
            this.networkObject = networkObject;

            if (encrypt)
                flags |= PacketFlag.Encrypted;
        }

        public override byte[] Construct(PacketEncryptor encryptor)
        {
            var packet = new NetworkObject();
            packet.AddField(0, NetworkObjectField.CreateIntField((int)opcode));
            packet.AddField(1, NetworkObjectField.CreateObjectField(networkObject));

            using (var stream = new MemoryStream())
            {
                using (var writer = new BinaryWriter(stream))
                {
                    writer.Write(0u);
                    writer.WritePackedUInt32((uint)flags);
                    writer.Write(NetworkObject.Pack(packet));

                    int length = (int)stream.Length - sizeof(int);
                    writer.Seek(0, SeekOrigin.Begin);
                    writer.Write(length);

                    return stream.ToArray();
                }
            }
        }
    }
}
