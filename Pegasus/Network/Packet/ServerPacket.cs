using Pegasus.Cryptography;

namespace Pegasus.Network.Packet
{
    public abstract class ServerPacket
    {
        protected PacketFlag flags = PacketFlag.None;

        public abstract byte[] Construct(PacketEncryptor encryptor);
    }
}
