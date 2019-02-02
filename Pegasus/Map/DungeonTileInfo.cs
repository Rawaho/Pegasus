using System.Numerics;
using Pegasus.Database.Model;

namespace Pegasus.Map
{
    public class DungeonTileInfo
    {
        public uint Id { get; }
        public ushort LandBlockId { get; }
        public ushort TileId { get; }
        public Vector3 Origin { get; }
        public byte Rotation { get; }

        public DungeonTileInfo(DungeonTile dungeonTile)
        {
            Id          = dungeonTile.Id;
            LandBlockId = dungeonTile.LandBlockId;
            TileId      = dungeonTile.TileId;
            Origin      = new Vector3(dungeonTile.X, dungeonTile.Y, dungeonTile.Z);
            Rotation    = dungeonTile.Rotation;
        }
    }
}
