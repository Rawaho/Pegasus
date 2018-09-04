using System.IO;
using System.Numerics;

namespace Pegasus.DungeonGenerator.Datastore.Object.Shared
{
    public class Plane : IUnpackable
    {
        public Vector3 N { get; private set; }
        public float D { get; private set; }

        public void UnPack(BinaryReader reader)
        {
            N = new Vector3(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
            D = reader.ReadSingle();
        }
    }
}
