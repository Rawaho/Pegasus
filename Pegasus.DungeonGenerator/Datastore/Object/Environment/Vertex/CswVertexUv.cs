using System.IO;

namespace Pegasus.DungeonGenerator.Datastore.Object.Environment.Vertex
{
    public class CswVertexUv : IUnpackable
    {
        public float U { get; private set; }
        public float V { get; private set; }

        public void UnPack(BinaryReader reader)
        {
            U = reader.ReadSingle();
            V = reader.ReadSingle();
        }
    }
}
