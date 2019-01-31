using System.IO;

namespace Pegasus.Network.Packet.Update.Shared
{
    public class Comp : IReadable, IWritable
    {
        public string Name { get; private set; }
        public uint Count { get; private set; }

        public void Read(BinaryReader reader)
        {
            Name  = reader.ReadPackedString();
            Count = reader.ReadPackedUInt32();
        }

        public void Write(BinaryWriter writer)
        {
            writer.WritePackedString(Name);
            writer.WritePackedUInt32(Count);
        }
    }
}
