using System.IO;

namespace Pegasus.Network.Packet.Object
{
    public abstract class Class3
    {
        public abstract void Read(BinaryReader reader);
        public abstract void Write(BinaryWriter writer);
        public abstract NetworkObject ToNetworkObject();
        public abstract void FromNetworkObject(NetworkObject class63_0);
    }
}
