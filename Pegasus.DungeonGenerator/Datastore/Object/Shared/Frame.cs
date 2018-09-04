using System.IO;
using System.Numerics;

namespace Pegasus.DungeonGenerator.Datastore.Object.Shared
{
    public class Frame : IUnpackable
    {
        public Vector3 Origin { get; private set; }
        public Quaternion Rotation { get; private set; }

        public void UnPack(BinaryReader reader)
        {
            Origin   = reader.ReadVector3();
            Rotation = reader.ReadQuaternion();
        }
    }
}
