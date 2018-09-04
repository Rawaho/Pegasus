using System;
using System.Collections.Generic;
using Pegasus.Database.Data;
using Pegasus.Map;
using Pegasus.Network.Packet;
using Pegasus.Network.Packet.Raw;

namespace Pegasus.Network.Handler
{
    public static class MapHandler
    {
        [ObjectPacketHandler(ObjectOpcode.const_12)]
        public static void Handle0C(Session session, NetworkObject networkObject)
        {
            int action = NetworkObjectField.ReadIntField(networkObject.GetField(0));
            //Console.WriteLine($"Action: {action}");

            switch (action)
            {
                case 7:
                {
                    break;
                }
                case 9:
                {
                    ushort landblockId = NetworkObjectField.ReadUShortField(networkObject.GetField(1));
                    break;
                }
            }
        }

        [RawPacketHandler(ClientRawOpcode.DungeonList)]
        public static void HandleDungeonList(Session session, ClientDungeonList packet)
        {
            var dungeonList = new ServerDungeonList
            {
                SearchParameter = packet.SearchParameter,
            };

            foreach (DungeonInfo dungeonInfo in DungeonTileManager.GetDungeonInfo(packet.SearchParameter))
                dungeonList.Dungeons.Add(new ServerDungeonList.Dungeon(dungeonInfo.LandBlockId, dungeonInfo.Name));

            session.EnqueuePacket(dungeonList);
        }








        [RawPacketHandler(ClientRawOpcode.const_3)]
        public static void HandleBla(Session session, ClientPacket04 packet)
        {
            foreach (uint cellId in packet.CellIds)
            {
                var dungeonTiles = new ServerPacket03();

                List<DungeonTileInfo> tiles = DungeonTileManager.GetDungeonTileInfo((ushort)cellId);
                if (tiles == null)
                    continue;

                foreach (DungeonTileInfo dungeonTile in tiles)
                {
                    dungeonTiles.Tiles.Add(new ServerPacket03.Tile
                    {
                        X           = dungeonTile.Origin.X,
                        Y           = dungeonTile.Origin.Y,
                        Z           = dungeonTile.Origin.Z,
                        TileId      = dungeonTile.TileId,
                        LandBlockId = dungeonTile.LandBlockId,
                        ushort_2 = 0,
                        byte_0 = 1
                    });




                }

                session.EnqueuePacket(dungeonTiles);
            }
        }
    }
}
