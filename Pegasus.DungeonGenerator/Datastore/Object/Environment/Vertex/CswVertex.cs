using System.IO;
using System.Numerics;

namespace Pegasus.DungeonGenerator.Datastore.Object.Environment.Vertex
{
    public class CswVertex : IUnpackable
    {
        public ushort VertId { get; private set; }
        public Vector3 Vertex { get; private set; }
        public Vector3 Normal { get; private set; }
        public CswVertexUv[] Uvs { get; private set; }

        public void UnPack(BinaryReader reader)
        {
            VertId = reader.ReadUInt16();
            Uvs    = new CswVertexUv[reader.ReadUInt16()];
            Vertex = new Vector3(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
            Normal = new Vector3(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());

            for (int i = 0; i < Uvs.Length; i++)
            {
                Uvs[i] = new CswVertexUv();
                Uvs[i].UnPack(reader);
            }
        }
    }
}
