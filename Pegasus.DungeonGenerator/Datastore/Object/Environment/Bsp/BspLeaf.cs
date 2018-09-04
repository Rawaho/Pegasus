using System.IO;
using Pegasus.DungeonGenerator.Datastore.Object.Shared;

namespace Pegasus.DungeonGenerator.Datastore.Object.Environment.Bsp
{
    public class BspLeaf : BspNode
    {
        public int LeafIndex { get; private set; }
        public uint Solid { get; private set; }

        public BspLeaf()
            : base(BspNodeType.Leaf)
        {
        }

        public override bool UnPack(BinaryReader reader)
        {
            LeafIndex = reader.ReadInt32();

            if (PackTreeType != BspTreeType.Physics)
                return true;

            Solid = reader.ReadUInt32();
            Sphere.UnPack(reader);

            uint numPolys = reader.ReadUInt32();
            if (numPolys > 0u)
            {
                InPolys = new CPolygon[numPolys];
                for (uint i = 0u; i < numPolys; i++)
                    InPolys[i] = PackPolys[reader.ReadUInt16()];
            }

            return true;
        }
    }
}
