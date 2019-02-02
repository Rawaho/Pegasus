using System.IO;

namespace Pegasus.Network.Packet.Object
{
    public class Class6 : Class4
    {
        public int Level = 0;
        public string string_0 = "";

        public override void Read(BinaryReader reader)
        {
            Class16 class2 = new Class16();
            class2.Read(reader);
            string_0 = class2.string_0;
            WrappedStruct<int> struct2 = new WrappedStruct<int>();
            struct2.Read(reader);
            Level = struct2.Value;
        }

        public override void Write(BinaryWriter writer)
        {
            new Class16 { string_0 = this.string_0 }.Write(writer);
            new WrappedStruct<int> { Value = this.Level }.Write(writer);
        }
    }
}
