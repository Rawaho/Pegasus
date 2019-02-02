using System.IO;

namespace Pegasus.Network.Packet.Update.Model
{
    public abstract class UpdateVital : IReadable, IWritable
    {
        public uint Maximum { get; set; }
        public uint Current { get; set; }

        public void Read(BinaryReader reader)
        {
            Maximum = reader.ReadPackedUInt32();
            Current = reader.ReadPackedUInt32();
            reader.ReadPackedUInt32();
        }

        public void Write(BinaryWriter writer)
        {
            writer.WritePackedUInt32(0);
            writer.WritePackedUInt32(Current);
            writer.WritePackedUInt32(Maximum);
        }
    }
}
