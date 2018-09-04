using System.IO;
using System.Text;

namespace Pegasus.Network
{
    public class WrappedPackedString : Class4
    {
        public string Value { get; set; } = "";

        public override void Read(BinaryReader reader)
        {
            WrappedPackedUInt32 length = new WrappedPackedUInt32();
            length.Read(reader);
            byte[] buffer = new byte[length.Value];
            reader.Read(buffer, 0, (int)length.Value);

            Value = Encoding.ASCII.GetString(buffer);
        }

        public override void Write(BinaryWriter writer)
        {
            byte[] bytes = Encoding.ASCII.GetBytes(this.Value);
            new WrappedPackedUInt32 { Value = (uint)bytes.Length }.Write(writer);
            writer.Write(bytes, 0, bytes.Length);
        }
    }
}
