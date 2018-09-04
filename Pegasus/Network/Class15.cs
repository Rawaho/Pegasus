using System.IO;

namespace Pegasus.Network
{
    public abstract class Class15 : Class3
    {
        public override void Read(BinaryReader reader)
        {
            int count = reader.ReadInt32();
            NetworkObject networkObject = NetworkObject.UnPack(reader.ReadBytes(count));
            FromNetworkObject(networkObject);
        }

        public override void Write(BinaryWriter writer)
        {
            byte[] data = NetworkObject.Pack(ToNetworkObject());
            writer.Write(data.Length);
            writer.Write(data);
        }
    }
}
