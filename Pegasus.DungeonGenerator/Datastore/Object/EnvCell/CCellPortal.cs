using System.IO;

namespace Pegasus.DungeonGenerator.Datastore.Object.EnvCell
{
    public class CCellPortal : IUnpackable
    {
        public bool ExactMatch { get; private set; }
        public bool PortalSide { get; private set; }
        public ushort PolyId { get; private set; }
        public ushort OtherCellId { get; private set; }
        public ushort OtherPortalId { get; private set; }

        public void UnPack(BinaryReader reader)
        {
            uint flags    = reader.ReadUInt16();
            ExactMatch    = (flags & 0x01) != 0;
            PortalSide    = (flags & 0x02) != 0;
            PolyId        = reader.ReadUInt16();
            OtherCellId   = reader.ReadUInt16();
            OtherPortalId = reader.ReadUInt16();
        }
    }
}
