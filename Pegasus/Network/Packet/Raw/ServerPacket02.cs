using System.IO;

namespace Pegasus.Network.Packet.Raw
{
    public class ServerPacket02 : ServerRawPacket
    {
        public struct WorldObject
        {
            public double double_0;
            public double double_1;
            public double double_2;
            public int int_0;
            public int int_1;
            public int int_2;
            public int int_3;
            public int int_4;
            public string string_0;
            public string string_1;
            public string string_2;
        }

        public ServerPacket02()
            : base(ServerRawOpcode.const_1)
        {
        }

        public override void Write(BinaryWriter writer)
        {
            writer.WritePackedUInt32(1);


            /*writer.Write(int_0);
            writer.Write(int_1);
            writer.Write(int_2);
            writer.Write(int_3);
            writer.Write(double_0);
            writer.Write(double_1);
            writer.Write(double_2);
            writer.WritePackedString(string_0);
            writer.WritePackedString(string_1);
            writer.WritePackedString(string_2);*/
        }
    }
}
