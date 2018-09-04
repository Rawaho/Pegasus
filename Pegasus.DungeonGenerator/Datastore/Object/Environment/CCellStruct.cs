using System.IO;
using System.Numerics;
using Pegasus.DungeonGenerator.Datastore.Object.Environment.Bsp;
using Pegasus.DungeonGenerator.Datastore.Object.Environment.Vertex;
using Pegasus.DungeonGenerator.Datastore.Object.Shared;

namespace Pegasus.DungeonGenerator.Datastore.Object.Environment
{
    public class CCellStruct : IUnpackable
    {
        public uint CellStructId { get; private set; }
        public CPolygon[] Polygons { get; private set; }
        public CPolygon[] PhysicsPolygons { get; private set; }
        public CPolygon[] Portals { get; private set; }
        public BspTree DrawingBsp { get; } = new BspTree();
        public BspTree PhysicsBsp { get; } = new BspTree();
        public BspTree CellBsp { get; } = new BspTree();
        public CVertexArray VertexArray { get; } = new CVertexArray();

        public void UnPack(BinaryReader reader)
        {
            CellStructId    = reader.ReadUInt32();
            Polygons        = new CPolygon[reader.ReadUInt32()];
            PhysicsPolygons = new CPolygon[reader.ReadUInt32()];
            Portals         = new CPolygon[reader.ReadUInt32()];

            VertexArray.UnPack(reader);

            for (int i = 0; i < Polygons.Length; i++)
            {
                Polygons[i] = new CPolygon();
                Polygons[i].UnPack(reader);
            }

            for (int i = 0; i < Portals.Length; i++)
                Portals[i] = Polygons[reader.ReadUInt16()];

            reader.Align();

            BspNode.PackPolys    = Polygons;
            BspNode.PackTreeType = BspTreeType.Cell;
            CellBsp.UnPack(reader);

            for (int i = 0; i < PhysicsPolygons.Length; i++)
            {
                PhysicsPolygons[i] = new CPolygon();
                PhysicsPolygons[i].UnPack(reader);
            }

            BspNode.PackPolys    = PhysicsPolygons;
            BspNode.PackTreeType = BspTreeType.Physics;
            PhysicsBsp.UnPack(reader);

            if (reader.ReadUInt32() != 0u)
            {
                BspNode.PackPolys    = Polygons;
                BspNode.PackTreeType = BspTreeType.Drawing;
                DrawingBsp.UnPack(reader);
            }

            reader.Align();
        }

        public bool PointInCell(Vector3 origin)
        {
            return CellBsp.PointInsideCellBsp(origin);
        }
    }
}
