using System.IO;

namespace Pegasus.DungeonGenerator.Datastore.Object.Environment.Vertex
{
    public class CVertexArray : IUnpackable
    {
        public VertexType Type { get; private set; }
        public CswVertex[] Vertices { get; private set; }

        public void UnPack(BinaryReader reader)
        {
            Type = (VertexType)reader.ReadUInt32();

            Vertices = new CswVertex[reader.ReadInt32()];
            for (int i = 0; i < Vertices.Length; i++)
            {
                Vertices[i] = new CswVertex();
                Vertices[i].UnPack(reader);
            }
        }
    }
}
