using System.IO;

namespace Pegasus.Network.Packet.Raw.Model
{
    [ServerRawMessage(ServerRawOpcode.const_1)]
    public class ServerPacket02 : IWritable
    {
        public class WorldObject : IWritable
        {
            public double double_0 { get; set; }
            public double double_1 { get; set; }
            public double double_2 { get; set; }
            public int int_0 { get; set; }
            public int int_1 { get; set; }
            public int int_2 { get; set; }
            public int int_3 { get; set; }
            public int int_4 { get; set; }
            public string string_0 { get; set; }
            public string string_1 { get; set; }
            public string string_2 { get; set; }

            public void Write(BinaryWriter writer)
            {
                writer.Write(int_0);
                writer.Write(int_1);
                writer.Write(int_2);
                writer.Write(int_3);
                writer.Write(double_0);
                writer.Write(double_1);
                writer.Write(double_2);
                writer.WritePackedString(string_0);
                writer.WritePackedString(string_1);
                writer.WritePackedString(string_2);
            }
        }

        public void Write(BinaryWriter writer)
        {
            writer.WritePackedUInt32(1);
        }
    }
}
