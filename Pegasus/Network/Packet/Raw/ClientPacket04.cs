using System.Collections.Generic;
using System.IO;

namespace Pegasus.Network.Packet.Raw
{
    [ClientRawPacket(ClientRawOpcode.const_3)]
    public class ClientPacket04 : ClientRawPacket
    {
        public List<uint> CellIds { get; } = new List<uint>();

        public override void Read(BinaryReader reader)
        {
            uint count = reader.ReadPackedUInt32();
            for (uint i = 0u; i < count; i++)
                CellIds.Add(reader.ReadUInt32());
        }
    }
}
