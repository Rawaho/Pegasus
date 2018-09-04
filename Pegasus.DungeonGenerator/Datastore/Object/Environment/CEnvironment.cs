using System.Diagnostics;
using System.IO;
using System.Linq;
using Pegasus.DungeonGenerator.Datastore.Object.Environment.Vertex;
using Pegasus.DungeonGenerator.Datastore.Object.Shared;

namespace Pegasus.DungeonGenerator.Datastore.Object.Environment
{
    public class CEnvironment : IUnpackable
    {
        public CCellStruct[] Cells { get; private set; }

        public void UnPack(BinaryReader reader)
        {
            uint bLoaded = reader.ReadUInt32();

            Cells = new CCellStruct[reader.ReadUInt32()];
            for (int i = 0; i < Cells.Length; i++)
            {
                Cells[i] = new CCellStruct();
                Cells[i].UnPack(reader);
            }
        }

        /// <summary>
        /// Write mesh information in Wavefront obj format.
        /// </summary>
        [Conditional("DEBUG")]
        public void WriteMesh(StringWriter writer)
        {
            foreach (CCellStruct cell in Cells)
            {
                foreach (CswVertex vertex in cell.VertexArray.Vertices.Reverse())
                    writer.WriteLine($"v {vertex.Vertex.X} {vertex.Vertex.Y} {vertex.Vertex.Z}");

                foreach (CPolygon polygon in cell.Polygons)
                    writer.WriteLine($"f -{string.Join(" -", polygon.VertexIds.Select(v => v + 1))}");
            }
        }
    }
}
