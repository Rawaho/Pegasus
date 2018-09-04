using System.IO;
using System.Numerics;

namespace Pegasus.DungeonGenerator.Datastore.Object.Environment.Bsp
{
    public class BspTree : IUnpackable
    {
        public BspNode Root { get; private set; }

        public void UnPack(BinaryReader reader)
        {
            BspNode.UnPackChild(out BspNode root, reader);
            Root = root;
        }

        public bool PointInsideCellBsp(Vector3 origin)
        {
            return Root.PointInsideCellBsp(origin);
        }
    }
}
