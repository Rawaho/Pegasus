using System.Collections.Generic;
using System.IO;

namespace Pegasus.Network.Packet.Raw
{
    [ClientRawPacket(ClientRawOpcode.CompList)]
    public class ClientCompList : ClientRawPacket
    {
        public List<string> Items { get; } = new List<string>();

        public override void Read(BinaryReader reader)
        {
            reader.ReadPackedUInt32(); // some enum value, always 1
            reader.ReadPackedUInt32(); // buffer length

            uint count = reader.ReadPackedUInt32();
            for (uint i = 0u; i < count; i++)
                Items.Add(reader.ReadPackedString());
        }
    }
}
