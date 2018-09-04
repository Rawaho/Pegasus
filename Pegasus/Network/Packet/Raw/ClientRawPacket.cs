using System.IO;

namespace Pegasus.Network.Packet.Raw
{
    public abstract class ClientRawPacket
    {
        public abstract void Read(BinaryReader reader);
    }
}
