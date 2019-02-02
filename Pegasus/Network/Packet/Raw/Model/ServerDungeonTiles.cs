using System.Collections.Generic;
using System.IO;

namespace Pegasus.Network.Packet.Raw.Model
{
    [ServerRawMessage(ServerRawOpcode.DungeonTiles)]
    public class ServerDungeonTiles : IWritable
    {
        public class Tile : IWritable
        {
            public float X { get; set; }
            public float Y { get; set; }
            public float Z { get; set; }
            public float float_3 { get; set; }
            public float float_4 { get; set; }
            public float float_5 { get; set; }
            public float float_6 { get; set; }
            public ushort TileId { get; set; }
            public ushort LandBlockId { get; set; }
            public ushort ushort_2 { get; set; }
            public byte rotation { get; set; }

            public void Write(BinaryWriter writer)
            {
                writer.Write(X);
                writer.Write(Y);
                writer.Write(Z);
                writer.Write(float_3);
                writer.Write(float_4);
                writer.Write(float_5);
                writer.Write(float_6);
                writer.Write(TileId);
                writer.Write(LandBlockId);
                writer.Write(ushort_2);
                writer.Write(rotation);
            }
        }

        public List<Tile> Tiles { get; } = new List<Tile>();

        public void Write(BinaryWriter writer)
        {
            writer.WritePackedUInt32((uint)Tiles.Count);
            Tiles.ForEach(t => t.Write(writer));
        }
    }
}
