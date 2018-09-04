using System.IO;

namespace Pegasus.DungeonGenerator.Datastore
{
    public interface IUnpackable
    {
        void UnPack(BinaryReader reader);
    }
}
