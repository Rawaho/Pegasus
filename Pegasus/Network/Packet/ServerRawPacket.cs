using System.IO;
using Pegasus.Cryptography;
using Pegasus.Network.Packet.Raw;

namespace Pegasus.Network.Packet
{
    public abstract class ServerRawPacket : ServerPacket
    {
        private readonly ServerRawOpcode opcode;

        protected ServerRawPacket(ServerRawOpcode opcode, bool encrypted = false)
        {
            this.opcode = opcode;

            flags |= PacketFlag.Raw;
            if (encrypted)
                flags |= PacketFlag.Encrypted;
        }

        public abstract void Write(BinaryWriter writer);

        public override byte[] Construct(PacketEncryptor encryptor)
        {
            using (var stream = new MemoryStream())
            {
                using (var writer = new BinaryWriter(stream))
                {
                    writer.Write(0u);
                    writer.WritePackedUInt32((uint)flags);
                    writer.WritePackedUInt32((uint)opcode);
                    Write(writer);

                    int length = (int)stream.Length - sizeof(int);
                    writer.Seek(0, SeekOrigin.Begin);
                    writer.Write(length);

                    return stream.ToArray();
                }
            }
        }
    }
}
