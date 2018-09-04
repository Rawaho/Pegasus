using System.Data;
using System.Numerics;

namespace Pegasus.Database.Data
{
    public class DungeonTileInfo : IReadable
    {
        public ushort LandBlockId { get; private set; }
        public ushort TileId { get; private set; }
        public Vector3 Origin { get; private set; }

        public void Read(DataRow row)
        {
            LandBlockId = row.Read<ushort>("landBlockId");
            TileId      = row.Read<ushort>("tileId");
            Origin      = new Vector3(row.Read<float>("x"), row.Read<float>("y"), row.Read<float>("z"));
        }
    }
}
