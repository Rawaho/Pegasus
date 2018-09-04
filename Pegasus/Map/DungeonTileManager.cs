using System;
using System.Collections.Generic;
using NLog;
using Pegasus.Database;
using Pegasus.Database.Data;

namespace Pegasus.Map
{
    public static class DungeonTileManager
    {
        private static readonly Logger log = LogManager.GetCurrentClassLogger();

        public class Dungeon
        {
            public DungeonInfo Info { get; }
            public List<DungeonTileInfo> Tiles { get; } = new List<DungeonTileInfo>();

            public Dungeon(DungeonInfo info)
            {
                Info = info;
            }
        }

        private static readonly Dictionary<ushort, Dungeon> dungeons = new Dictionary<ushort, Dungeon>();

        public static void Initialise()
        {
            log.Info("Loading Dungeon tiles...");

            foreach (DungeonInfo dungeon in DatabaseManager.Database.GetDungeons())
                dungeons.Add(dungeon.LandBlockId, new Dungeon(dungeon));

            foreach (DungeonTileInfo dungeonTile in DatabaseManager.Database.GetDungeonTiles())
                if (dungeons.TryGetValue(dungeonTile.LandBlockId, out Dungeon dungeon))
                    dungeon.Tiles.Add(dungeonTile);
        }

        public static IEnumerable<DungeonInfo> GetDungeonInfo(string search)
        {
            string substring = search.Substring(1, search.Length - 2);
            foreach (KeyValuePair<ushort, Dungeon> dungeon in dungeons)
                if (dungeon.Value.Info.Name.IndexOf(substring, StringComparison.CurrentCultureIgnoreCase) >= 0)
                    yield return dungeon.Value.Info;
        }

        public static List<DungeonTileInfo> GetDungeonTileInfo(ushort landBlockId)
        {
            return !dungeons.TryGetValue(landBlockId, out Dungeon dungeon) ? null : dungeon.Tiles;
        }
    }
}
