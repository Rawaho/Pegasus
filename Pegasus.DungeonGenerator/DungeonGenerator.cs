using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using NLog;
using Pegasus.DungeonGenerator.Datastore;
using Pegasus.DungeonGenerator.Datastore.Object.LandblockInfo;

namespace Pegasus.DungeonGenerator
{
    internal static class DungeonGenerator
    {
        #if DEBUG
            public const string Title = "Pegasus: Dungeon Generator (Debug)";
        #else
            public const string Title = "Pegasus: Dungeon Generator (Release)";
        #endif

        private static readonly Logger log = LogManager.GetCurrentClassLogger();

        private static void Main()
        {
            Directory.SetCurrentDirectory(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location));

            Console.Title = Title;

            DatastoreManager.Initialise();
            foreach (CLandBlockInfo landBlockInfo in DatastoreManager.CellDatastore.GetLandBlockInfo())
                if (landBlockInfo.IsDungeon())
                    GenerateLandBlockTiles(landBlockInfo);

            log.Info("Finished!");
            Console.ReadLine();
        }

        private static void GenerateLandBlockTiles(CLandBlockInfo landBlockInfo)
        {
            var sw = new StringWriter();
            sw.WriteLine($"INSERT INTO `dungeon` (`landBlockId`, `name`) VALUES ({landBlockInfo.LandBlockId}, 'Dungeon{landBlockInfo.LandBlockId:X4}');");
            sw.WriteLine("INSERT INTO `dungeon_tile` (`landBlockId`, `tileId`, `x`, `y`, `z`) VALUES");

            // 65 is just a place holder, need to match the cell environment with the 2D counterpart
            List<string> tiles = landBlockInfo.Cells
                .Select(c => $"({landBlockInfo.LandBlockId}, 65, {c.Position.Origin.X}, {c.Position.Origin.Y}, {c.Position.Origin.Z})")
                .Distinct()
                .ToList();

            sw.Write(string.Join($",{Environment.NewLine}", tiles));
            sw.WriteLine(";");

            System.IO.File.WriteAllText($"sql/{landBlockInfo.LandBlockId:X4}.sql", sw.ToString());
            log.Info($"{landBlockInfo.LandBlockId:X4}");
        }
    }
}
