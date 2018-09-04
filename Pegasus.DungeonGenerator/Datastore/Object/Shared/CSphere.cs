using System.IO;
using System.Numerics;

namespace Pegasus.DungeonGenerator.Datastore.Object.Shared
{
    public class CSphere : IUnpackable
    {
        public Vector3 Center { get; private set; }
        public float Radius { get; private set; }

        public void UnPack(BinaryReader reader)
        {
            Center = new Vector3(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
            Radius = reader.ReadSingle();
        }
    }
}
