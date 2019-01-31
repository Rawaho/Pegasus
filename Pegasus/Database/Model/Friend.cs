using System;
using System.Collections.Generic;

namespace Pegasus.Database.Model
{
    public partial class Friend
    {
        public uint Id { get; set; }
        public uint Friend1 { get; set; }
        public DateTime AddTime { get; set; }

        public virtual Account Friend1Navigation { get; set; }
        public virtual Account IdNavigation { get; set; }
    }
}
