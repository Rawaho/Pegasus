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

        private static Dictionary<int, string> dungeons;

        private static void Main()
        {
            Directory.SetCurrentDirectory(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location));

            Console.Title = Title;

            dungeons = System.IO.File.ReadLines("dungeons.csv").Select(line => line.Split(',')).ToDictionary(line => Int32.Parse(line[0], System.Globalization.NumberStyles.HexNumber), line => line[1]);

            DatastoreManager.Initialise();
            foreach (CLandBlockInfo landBlockInfo in DatastoreManager.CellDatastore.GetLandBlockInfo())
                if (landBlockInfo.IsDungeon())
                    GenerateLandBlockTiles(landBlockInfo);

            log.Info("Finished!");
            Console.ReadLine();
        }

        private static string GetDungeonName(CLandBlockInfo landblockInfo) {
            if (dungeons.ContainsKey(landblockInfo.LandBlockId) && !String.IsNullOrEmpty(dungeons[landblockInfo.LandBlockId])) {
                return dungeons[landblockInfo.LandBlockId];
            }

            return "Dungeon" + landblockInfo.LandBlockId.ToString("X");
        }

        private static void GenerateLandBlockTiles(CLandBlockInfo landBlockInfo) {
            var sw = new StringWriter();
            sw.WriteLine($"INSERT INTO `dungeon` (`landBlockId`, `name`) VALUES ({landBlockInfo.LandBlockId}, '{GetDungeonName(landBlockInfo).Replace("'", @"\'")}');");
            sw.WriteLine("INSERT INTO `dungeon_tile` (`landBlockId`, `tileId`, `x`, `y`, `z`, `rotation`) VALUES");
            
            List<string> tiles = landBlockInfo.Cells
                .Select(c => {
                    byte rotation = 0;

                    if (c.Position.Rotation.W == 1) {
                        rotation = 4;
                    }
                    else if (c.Position.Rotation.W < -0.70 && c.Position.Rotation.W > -0.8) {
                        rotation = 7;
                    }
                    else if (c.Position.Rotation.W < 0.10 && c.Position.Rotation.W > -0.1) {
                        rotation = 2;
                    }
                    else if (c.Position.Rotation.W > 0.70 && c.Position.Rotation.W < 0.8) {
                        rotation = 1;
                    }

                    ushort eid = c.EnvironmentId;

                    // seems like most EnvironmentIds match up to vi2 tile names
                    // why are some of these not matching?
                    switch (eid) {
                        // room with 4 doors
                        case 258:
                            eid = 465;
                            break;

                        // stairs?
                        case 19:
                            eid = 298;
                            break;

                        // 3 window bridge/tunnel thing e/w direction? (facility hub)
                        case 679:
                            eid = 671;
                            rotation = 1;
                            break;

                        // 3 window bridge/tunnel thing n/s direction? (facility hub)
                        case 672:
                            eid = 671;
                            rotation = (rotation == 2 || rotation == 4) ? (byte)1 : (byte)2;
                            break;

                        // probably a better way to do this?
                        // need to figure out if a cell has a floor and we can get all of these in one go
                        case 80:
                            return "";
                        case 202:
                            return "";
                        case 149:
                            return "";
                        case 367:
                            return "";
                        case 217:
                            return "";
                        case 642:
                            return "";
                        case 652:
                            return "";
                        case 653:
                            return "";
                        case 636:
                            return "";
                        case 665:
                            return "";
                        case 646:
                            return "";
                        case 637:
                            return "";
                        case 654:
                            return "";
                        case 664:
                            return "";
                        case 638:
                            return "";
                    }

                    return $"({landBlockInfo.LandBlockId}, {eid}, {c.Position.Origin.X}, {c.Position.Origin.Y}, {c.Position.Origin.Z}, {rotation})";
                })
                .Where(c => { return !String.IsNullOrEmpty(c); })
                .Distinct()
                .ToList();

            sw.Write(string.Join($",{Environment.NewLine}", tiles));
            sw.WriteLine(";");

            System.IO.File.WriteAllText($"sql/{landBlockInfo.LandBlockId:X4}.sql", sw.ToString());
            log.Info($"{landBlockInfo.LandBlockId:X4}");
        }
    }
}
