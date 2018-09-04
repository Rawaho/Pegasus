using System.IO;
using Pegasus.DungeonGenerator.Datastore.Object.Environment;
using Pegasus.DungeonGenerator.Datastore.Object.Shared;

namespace Pegasus.DungeonGenerator.Datastore.Object.EnvCell
{
    public class CEnvCell : IUnpackable
    {
        public uint CellId { get; private set; }
        public uint Flags { get; private set; }
        public uint[] Surfaces { get; private set; }
        public CCellPortal[] Portals { get; private set; }
        public ushort[] VisibleBlocks { get; private set; }
        public ushort EnvironmentId { get; private set; }
        public ushort CellStructure { get; private set; }
        public Frame Position { get; } = new Frame();
        public uint[] StaticObjectIds { get; private set; }
        public Frame[] StaticObjectFrames { get; private set; }

        private CEnvironment environment;
        private CCellStruct structure;

        public void UnPack(BinaryReader reader)
        {
            CellId = reader.ReadUInt32();
            Flags  = reader.ReadUInt32();
            reader.Skip(sizeof(uint));

            Surfaces = new uint[reader.ReadByte()];
            Portals  = new CCellPortal[reader.ReadByte()];
            VisibleBlocks = new ushort[reader.ReadUInt16()];

            for (int i = 0; i < Surfaces.Length; i++)
                Surfaces[i] = (uint)reader.ReadUInt16() | 0x08000000;

            EnvironmentId = reader.ReadUInt16();
            CellStructure = reader.ReadUInt16();

            environment   = DatastoreManager.PortalDataStore.GetEnvironment(EnvironmentId | 0x0D000000u);
            structure     = environment.Cells[CellStructure];

            Position.UnPack(reader);

            for (int i = 0; i < Portals.Length; i++)
            {
                Portals[i] = new CCellPortal();
                Portals[i].UnPack(reader);
            }

            for (int i = 0; i < VisibleBlocks.Length; i++)
                VisibleBlocks[i] = reader.ReadUInt16();

            if ((Flags & 0x02) != 0)
            {
                uint numStaticObjects = reader.ReadUInt32();
                StaticObjectIds    = new uint[numStaticObjects];
                StaticObjectFrames = new Frame[numStaticObjects];

                for (int i = 0; i < StaticObjectIds.Length; i++)
                {
                    StaticObjectIds[i] = reader.ReadUInt32();

                    StaticObjectFrames[i] = new Frame();
                    StaticObjectFrames[i].UnPack(reader);
                }
            }

            if ((Flags & 0x08) != 0)
                reader.ReadUInt32();
        }
    }
}
