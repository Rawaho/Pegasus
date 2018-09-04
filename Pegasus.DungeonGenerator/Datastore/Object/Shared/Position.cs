using System.IO;

namespace Pegasus.DungeonGenerator.Datastore.Object.Shared
{
    public class Position : IUnpackable
    {
        public uint CellId { get; private set; }
        public Frame Frame { get; } = new Frame();

        public void UnPack(BinaryReader reader)
        {
            Frame.UnPack(reader);
            CellId = reader.ReadUInt32();
        }
    }
}
