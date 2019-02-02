using System.Collections.Generic;
using System.IO;

namespace Pegasus.Network.Packet.Update.Model
{
    [UpdateMessage(UpdateType.Comps)]
    public class UpdateComps : IReadable, IWritable
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

        public List<Comp> Comps { get; set; } = new List<Comp>();

        public void Read(BinaryReader reader)
        {
            uint count = reader.ReadUInt32();
            for (uint i = 0u; i < count; i++)
            {
                var comp = new Comp();
                comp.Read(reader);
                Comps.Add(comp);
            }
        }

        public void Write(BinaryWriter writer)
        {
            writer.Write(Comps.Count);
            Comps.ForEach(c => c.Write(writer));
        }
    }
}
