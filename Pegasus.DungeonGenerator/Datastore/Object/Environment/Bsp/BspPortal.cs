using System;
using System.IO;

namespace Pegasus.DungeonGenerator.Datastore.Object.Environment.Bsp
{
    public class BspPortal : BspNode
    {
        public BspPortal()
            : base(BspNodeType.Port)
        {
        }

        public override bool UnPack(BinaryReader reader)
        {
            throw new NotSupportedException();
            SplitingPlane.UnPack(reader);

            return true;
        }
    }
}
