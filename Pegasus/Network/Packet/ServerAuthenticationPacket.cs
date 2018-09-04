using System.IO;
using Pegasus.Cryptography;

namespace Pegasus.Network.Packet
{
    public class ServerAuthenticationPacket : ServerObjectPacket
    {
        public ServerAuthenticationPacket(NetworkObject networkObject)
            : base(ObjectOpcode.Authenticate, networkObject, false)
        {
        }

        public override byte[] Construct(PacketEncryptor encryptor)
        {
            using (var stream = new MemoryStream())
            {
                using (var writer = new BinaryWriter(stream))
                {
                    writer.Write(0u);
                    writer.WritePackedUInt32((uint)flags);
                    writer.Write(NetworkObject.Pack(networkObject));

                    int length = (int)stream.Length - sizeof(int);

                    writer.Seek(0, SeekOrigin.Begin);
                    writer.Write(length);

                    return stream.ToArray();
                }
            }
        }
    }
}
