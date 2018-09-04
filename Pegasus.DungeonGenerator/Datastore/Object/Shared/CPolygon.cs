using System.IO;

namespace Pegasus.DungeonGenerator.Datastore.Object.Shared
{
    public class CPolygon : IUnpackable
    {
        public enum SidesType
        {
            Single = 0x00,
            Double = 0x01,
            Both   = 0x02
        }

        public uint PolyId { get; private set; }
        public ushort[] VertexIds { get; private set; }
        public byte[] PosUvIndices { get; private set; }
        public byte[] NegUvIndices { get; private set; }
        public ushort PosSurface { get; private set; }
        public ushort NegSurface { get; private set; }
        public byte NumPts { get; private set; }
        public byte Strippling { get; private set; }
        public SidesType SideType { get; private set; }

        public void UnPack(BinaryReader reader)
        {
            PolyId     = reader.ReadUInt16();
            NumPts     = reader.ReadByte();
            Strippling = reader.ReadByte();
            SideType   = (SidesType)reader.ReadUInt32();
            PosSurface = reader.ReadUInt16();
            NegSurface = reader.ReadUInt16();

            VertexIds = new ushort[NumPts];
            for (byte i = 0; i < NumPts; i++)
                VertexIds[i] = reader.ReadUInt16();

            if ((Strippling & 0x04) == 0)
            {
                PosUvIndices = new byte[NumPts];
                for (byte i = 0; i < NumPts; i++)
                    PosUvIndices[i] = reader.ReadByte();
            }

            if ((Strippling & 0x08) == 0 && SideType == SidesType.Both)
            {
                NegUvIndices = new byte[NumPts];
                for (byte i = 0; i < NumPts; i++)
                    NegUvIndices[i] = reader.ReadByte();
            }
        }
    }
}
