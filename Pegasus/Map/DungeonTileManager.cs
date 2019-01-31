using System;
using System.Collections.Generic;
using NLog;
using Pegasus.Database;
using Pegasus.Database.Model;

namespace Pegasus.Map
{
    public static class DungeonTileManager
    {
        private static readonly Logger log = LogManager.GetCurrentClassLogger();

        private static readonly Dictionary<ushort, DungeonInfo> dungeons = new Dictionary<ushort, DungeonInfo>();

        public static void Initialise()
        {
            log.Info("Loading Dungeon tiles...");

            foreach (Dungeon dungeon in DatabaseManager.GetDungeons())
                dungeons.Add(dungeon.LandBlockId, new DungeonInfo(dungeon));
        }

        public static DungeonInfo GetDungeonInfo(ushort landBlockId)
        {
            return dungeons.TryGetValue(landBlockId, out DungeonInfo dungeonInfo) ? dungeonInfo : null;
        }

        public static IEnumerable<DungeonInfo> GetDungeonInfo(string search)
        {
            string substring = search.Substring(1, search.Length - 2);
            foreach (DungeonInfo dungeonInfo in dungeons.Values)
                if (dungeonInfo.Name.IndexOf(substring, StringComparison.CurrentCultureIgnoreCase) >= 0)
                    yield return dungeonInfo;
        }
    }
}
