using System.Collections.Generic;
using System.IO;

namespace Pegasus.Network.Packet.Raw
{
    [ClientRawPacket(ClientRawOpcode.CompList)]
    public class ClientCompList : ClientRawPacket
    {
        public class Comp : IReadable
        {
            public string Name { get; private set; }
            public uint Icon { get; private set; }

            public void Read(BinaryReader reader)
            {
                Name = reader.ReadPackedString();
                Icon = reader.ReadUInt32();
            }
        }

        public uint Key { get; private set; }
        public List<Comp> Comps { get; } = new List<Comp>();

        public override void Read(BinaryReader reader)
        {
            // dictonary key used to store comp buffer in plugin, always 1
            Key = reader.ReadPackedUInt32();
            reader.ReadPackedUInt32(); // buffer length

            uint count = reader.ReadPackedUInt32();
            for (uint i = 0u; i < count; i++)
            {
                var comp = new Comp();
                comp.Read(reader);
                Comps.Add(comp);
            }
        }
    }
}
