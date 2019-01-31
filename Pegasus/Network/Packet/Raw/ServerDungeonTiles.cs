using System.Collections.Generic;
using System.IO;

namespace Pegasus.Network.Packet.Raw
{
    public class ServerDungeonTiles : ServerRawPacket
    {
        public struct Tile
        {
            public byte byte_0;
            public float X;
            public float Y;
            public float Z;
            public float float_3;
            public float float_4;
            public float float_5;
            public float float_6;
            public ushort LandBlockId;
            public ushort TileId;
            public ushort ushort_2;
        }

        public ServerDungeonTiles()
            : base(ServerRawOpcode.const_2)
        {
        }

        public List<Tile> Tiles { get; } = new List<Tile>();

        public override void Write(BinaryWriter writer)
        {
            writer.WritePackedUInt32((uint)Tiles.Count);

            foreach (Tile tile in Tiles)
            {
                writer.Write(tile.X);
                writer.Write(tile.Y);
                writer.Write(tile.Z);
                writer.Write(tile.float_3);
                writer.Write(tile.float_4);
                writer.Write(tile.float_5);
                writer.Write(tile.float_6);
                writer.Write(tile.TileId);
                writer.Write(tile.LandBlockId);
                writer.Write(tile.ushort_2);
                writer.Write(tile.byte_0);
            }
        }
    }
}
