using System;
using System.Collections.Generic;

namespace Pegasus.Database.Model
{
    public partial class DungeonTile
    {
        public uint Id { get; set; }
        public ushort LandBlockId { get; set; }
        public ushort TileId { get; set; }
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }

        public virtual Dungeon LandBlock { get; set; }
    }
}
