using System;
using System.Collections.Generic;

namespace Pegasus.Database.Model
{
    public partial class Dungeon
    {
        public Dungeon()
        {
            DungeonTile = new HashSet<DungeonTile>();
        }

        public ushort LandBlockId { get; set; }
        public string Name { get; set; }

        public virtual ICollection<DungeonTile> DungeonTile { get; set; }
    }
}
