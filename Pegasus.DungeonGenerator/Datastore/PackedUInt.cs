using System.IO;

namespace Pegasus.DungeonGenerator.Datastore
{
    public struct PackedUInt : IUnpackable
    {
        public uint Value { get; private set; }

        public void UnPack(BinaryReader reader)
        {
            Value = reader.ReadUInt32();
        }
    }
}
