using System.IO;

namespace Pegasus.DungeonGenerator.Datastore.Object.LandblockInfo
{
    public class CBldPortal : IUnpackable
    {
        public bool ExactMatch { get; private set; }
        public bool PortalSide { get; private set; }
        public ushort OtherCellId { get; private set; }
        public ushort OtherPortalId { get; private set; }
        public uint[] SlabList { get; private set; }

        public void UnPack(BinaryReader reader)
        {
            uint flags    = reader.ReadUInt16();
            ExactMatch    = (flags & 0x01) != 0;
            PortalSide    = (flags & 0x02) != 0;
            OtherCellId   = reader.ReadUInt16();
            OtherPortalId = reader.ReadUInt16();

            SlabList = new uint[reader.ReadUInt16()];
            for (int i = 0; i < SlabList.Length; i++)
                SlabList[i] = reader.ReadUInt16();

            reader.Align();
        }
    }
}
