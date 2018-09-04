using System.IO;

namespace Pegasus.Network.Packet.Update
{
    public abstract class ClientUpdatePacket
    {
        public abstract void ReadUpdate(BinaryReader reader);
    }
}
