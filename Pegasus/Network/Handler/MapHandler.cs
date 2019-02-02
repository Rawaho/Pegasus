using Pegasus.Map;
using Pegasus.Network.Packet.Object;
using Pegasus.Network.Packet.Raw;
using Pegasus.Network.Packet.Raw.Model;

namespace Pegasus.Network.Handler
{
    public static class MapHandler
    {
        [ObjectPacketHandler(ObjectOpcode.const_12)]
        public static void Handle0C(Session session, NetworkObject networkObject)
        {
            int action = NetworkObjectField.ReadIntField(networkObject.GetField(0));
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

        [RawMessageHandler(ClientRawOpcode.DungeonList)]
        public static void HandleDungeonList(Session session, ClientDungeonList packet)
        {
            var dungeonList = new ServerDungeonList
            {
                SearchParameter = packet.SearchParameter
            };

            foreach (DungeonInfo dungeonInfo in DungeonTileManager.GetDungeonInfo(packet.SearchParameter))
                dungeonList.Dungeons.Add(new ServerDungeonList.Dungeon(dungeonInfo));

            session.EnqueueMessage(dungeonList);
        }

        [RawMessageHandler(ClientRawOpcode.DungeonTiles)]
        public static void HandleDungeonTile(Session session, ClientDungeonTiles packet)
        {
            foreach (uint cellId in packet.CellIds)
            {
                DungeonInfo dungeonInfo = DungeonTileManager.GetDungeonInfo((ushort)cellId);
                if (dungeonInfo == null)
                    continue;

                var dungeonTiles = new ServerDungeonTiles();
                foreach (DungeonTileInfo dungeonTileInfo in dungeonInfo)
                {
                    dungeonTiles.Tiles.Add(new ServerDungeonTiles.Tile
                    {
                        X           = dungeonTileInfo.Origin.X,
                        Y           = dungeonTileInfo.Origin.Y,
                        Z           = dungeonTileInfo.Origin.Z,
                        TileId      = dungeonTileInfo.TileId,
                        LandBlockId = dungeonTileInfo.LandBlockId,
                        ushort_2    = 0,
                        byte_0      = 1
                    });
                }

                session.EnqueueMessage(dungeonTiles);
            }
        }
    }
}
