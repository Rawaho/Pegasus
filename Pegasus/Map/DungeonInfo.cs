using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pegasus.Database.Model;

namespace Pegasus.Map
{
    public class DungeonInfo : IEnumerable<DungeonTileInfo>
    {
        public string Name { get; }
        public ushort LandBlockId { get; }

        private readonly List<DungeonTileInfo> tiles;

        public DungeonInfo(Dungeon dungeon)
        {
            Name        = dungeon.Name;
            LandBlockId = dungeon.LandBlockId;

            tiles = dungeon.DungeonTile
                .Select(t => new DungeonTileInfo(t))
                .ToList();
        }

        public IEnumerator<DungeonTileInfo> GetEnumerator()
        {
            return tiles.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
