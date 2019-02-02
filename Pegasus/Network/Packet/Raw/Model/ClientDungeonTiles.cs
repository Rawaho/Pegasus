using System.Collections.Generic;
using System.IO;

namespace Pegasus.Network.Packet.Raw.Model
{
    [ClientRawPacket(ClientRawOpcode.DungeonTiles)]
    public class ClientDungeonTiles : IReadable
    {
        public List<uint> CellIds { get; } = new List<uint>();

        public void Read(BinaryReader reader)
        {
            uint count = reader.ReadPackedUInt32();
            for (uint i = 0u; i < count; i++)
                CellIds.Add(reader.ReadUInt32());
        }
    }
}
