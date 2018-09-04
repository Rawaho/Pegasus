using System.IO;
using System.Numerics;
using Pegasus.DungeonGenerator.Datastore.Object.Shared;

namespace Pegasus.DungeonGenerator.Datastore.Object.Environment.Bsp
{
    public class BspNode
    {
        public static BspTreeType PackTreeType { get; set; }
        public static CPolygon[] PackPolys { get; set; }

        public BspNodeType Type { get; }
        public BspNode PosNode { get; private set; }
        public BspNode NegNode { get; private set; }
        public Shared.Plane SplitingPlane { get; } = new Shared.Plane();

        public CPolygon[] InPolys { get; set; }
        public CSphere Sphere { get; } = new CSphere();

        public BspNode(BspNodeType type)
        {
            Type = type;
        }

        public virtual bool UnPack(BinaryReader reader)
        {
            SplitingPlane.UnPack(reader);

            switch (Type)
            {
                case BspNodeType.BPnn:
                case BspNodeType.BPIn:
                {
                    if (!UnPackChild(out BspNode posNode, reader))
                        return false;
                    PosNode = posNode;
                    break;
                }
                case BspNodeType.BpIN:
                case BspNodeType.BpnN:
                {
                    if (!UnPackChild(out BspNode negNode, reader))
                        return false;
                    NegNode = negNode;
                    break;
                }
                case BspNodeType.BPIN:
                case BspNodeType.BPnN:
                {
                    if (!UnPackChild(out BspNode posNode, reader))
                        return false;
                    PosNode = posNode;
                    if (!UnPackChild(out BspNode negNode, reader))
                        return false;
                    NegNode = negNode;
                    break;
                }
            }

            if (PackTreeType != BspTreeType.Drawing)
            {
                if (PackTreeType == BspTreeType.Physics)
                    Sphere.UnPack(reader);

                return true;
            }

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

        public bool PointInsideCellBsp(Vector3 origin)
        {
            return true;
        }

        public static bool UnPackChild(out BspNode node, BinaryReader reader)
        {
            BspNodeType type = (BspNodeType)reader.ReadUInt32();
            switch (type)
            {
                case BspNodeType.Port:
                    node = new BspPortal();
                    break;
                case BspNodeType.Leaf:
                    node = new BspLeaf();
                    break;
                default:
                    node = new BspNode(type);
                    break;
            }

            return node.UnPack(reader);
        }
    }
}
