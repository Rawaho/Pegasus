using System.Collections.Generic;
using System.IO;
using Pegasus.Map;

namespace Pegasus.Network.Packet.Raw.Model
{
    [ServerRawMessage(ServerRawOpcode.DungeonList)]
    public class ServerDungeonList : IWritable
    {
        public class Dungeon
        {
            public ushort LandBlockId { get; }
            public string Name { get; }

            public Dungeon(DungeonInfo dungeonInfo)
            {
                LandBlockId = dungeonInfo.LandBlockId;
                Name        = dungeonInfo.Name;
            }
        }

        public string SearchParameter { get; set; }
        public List<Dungeon> Dungeons { get; } = new List<Dungeon>();

        public void Write(BinaryWriter writer)
        {
            writer.WritePackedString(SearchParameter);
            writer.WritePackedUInt32((uint)Dungeons.Count);
            foreach (Dungeon dungeon in Dungeons)
                writer.WritePackedString(dungeon.Name);
            foreach (Dungeon dungeon in Dungeons)
                writer.Write(dungeon.LandBlockId);
        }
    }
}
