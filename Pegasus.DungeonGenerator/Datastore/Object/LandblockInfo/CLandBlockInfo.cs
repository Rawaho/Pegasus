using System.IO;
using System.Linq;
using Pegasus.DungeonGenerator.Datastore.Container;
using Pegasus.DungeonGenerator.Datastore.Object.EnvCell;
using Pegasus.DungeonGenerator.Datastore.Object.Shared;

namespace Pegasus.DungeonGenerator.Datastore.Object.LandblockInfo
{
    public class CLandBlockInfo : IUnpackable
    {
        public ushort LandBlockId { get; private set; }
        public CEnvCell[] Cells { get; private set; }
        public uint[] ObjectIds { get; private set; }
        public Frame[] ObjectFrames { get; private set; }
        public BuildInfo[] Buildings { get; private set; }
        public uint Flags { get; private set; }
        public ReadOnlyPackableHashTable<PackedUInt> RestrictionTable { get; } = new ReadOnlyPackableHashTable<PackedUInt>();

        public void UnPack(BinaryReader reader)
        {
            LandBlockId = (ushort)(reader.ReadUInt32() >> 16);
            Cells = new CEnvCell[reader.ReadUInt32()];

            uint numObjects = reader.ReadUInt32();
            ObjectIds    = new uint[numObjects];
            ObjectFrames = new Frame[numObjects];

            for (int i = 0; i < numObjects; i++)
            {
                ObjectIds[i] = reader.ReadUInt32();
                ObjectFrames[i] = new Frame();
                ObjectFrames[i].UnPack(reader);
            }

            Buildings = new BuildInfo[reader.ReadUInt16()];
            Flags     = reader.ReadUInt16();

            for (int i = 0; i < Buildings.Length; i++)
            {
                Buildings[i] = new BuildInfo();
                Buildings[i].UnPack(reader);
            }

            /*if ((Flags & 0x01) != 0)
                RestrictionTable.UnPack(reader);*/
        }

        public bool IsDungeon()
        {
            return Cells.Length != 0 && Cells.All(envCell => envCell.Position.Origin.X % 10 == 0);
        }
    }
}
