using System.IO;

namespace Pegasus.Network
{
    public abstract class Class3
    {
        public abstract void Read(BinaryReader reader);
        public abstract void Write(BinaryWriter writer);
        public abstract NetworkObject ToNetworkObject();
        public abstract void FromNetworkObject(NetworkObject class63_0);
    }
}
