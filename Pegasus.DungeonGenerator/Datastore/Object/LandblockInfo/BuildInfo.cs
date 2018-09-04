using System.IO;
using Pegasus.DungeonGenerator.Datastore.Object.Shared;

namespace Pegasus.DungeonGenerator.Datastore.Object.LandblockInfo
{
    public class BuildInfo : IUnpackable
    {
        public uint BuildingId { get; private set; }
        public Frame BuildingFrame { get; } = new Frame();
        public uint NumLeaves { get; private set; }
        public CBldPortal[] Portals { get; private set; }

        public void UnPack(BinaryReader reader)
        {
            BuildingId = reader.ReadUInt32();
            BuildingFrame.UnPack(reader);
            NumLeaves  = reader.ReadUInt32();

            Portals = new CBldPortal[reader.ReadUInt32()];
            for (int i = 0; i < Portals.Length; i++)
            {
                Portals[i] = new CBldPortal();
                Portals[i].UnPack(reader);
            }
        }
    }
}
