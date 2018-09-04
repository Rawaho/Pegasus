using System;
using System.IO;

namespace Pegasus.Network
{
    public class WrappedPackedUInt32 : Class4
    {
        public uint Value { get; set; }

        public override void Read(BinaryReader reader)
        {
            byte num = (byte)reader.ReadByte();
            byte num2 = 0;
            byte num3 = 0;
            byte num4 = 0;
            bool flag = false;
            bool flag2 = false;
            bool flag3 = false;
            if ((num & 0x80) > 0)
            {
                num2 = (byte)reader.ReadByte();
                flag = true;
            }
            if ((num2 & 0x80) > 0)
            {
                num3 = (byte)reader.ReadByte();
                flag2 = true;
            }
            if ((num3 & 0x80) > 0)
            {
                num4 = (byte)reader.ReadByte();
                flag3 = true;
            }
            num = (byte)(num & 0x7f);
            num2 = (byte)(num2 & 0x7f);
            num3 = (byte)(num3 & 0x7f);
            this.Value = 0;
            if (flag3)
            {
                this.Value = num4;
                this.Value = this.Value << 7;
            }
            if (flag2)
            {
                this.Value |= num3;
                this.Value = this.Value << 7;
            }
            if (flag)
            {
                this.Value |= num2;
                this.Value = this.Value << 7;
            }
            this.Value |= num;
        }

        public override void Write(BinaryWriter writer)
        {
            if ((this.Value & 0xe0000000) > 0)
            {
                throw new OverflowException("WrappedPackedUInt32: maximum value exceeded. Upper 3 bits must be clear.");
            }

            bool flag = (this.Value & 0xffffff80) > 0;
            bool flag2 = (this.Value & 0xffffc000) > 0;
            bool flag3 = (this.Value & 0xffe00000) > 0;
            uint num = this.Value;
            byte num2 = (byte)(num & 0x7f);
            num = num >> 7;
            byte num3 = (byte)(num & 0x7f);
            num = num >> 7;
            byte num4 = (byte)(num & 0x7f);
            num = num >> 7;
            byte num5 = (byte)(num & 0xff);
            if (flag)
            {
                num2 = (byte)(num2 | 0x80);
            }
            if (flag2)
            {
                num3 = (byte)(num3 | 0x80);
            }
            if (flag3)
            {
                num4 = (byte)(num4 | 0x80);
            }
            writer.Write(num2);
            if (flag)
            {
                writer.Write(num3);
            }
            if (flag2)
            {
                writer.Write(num4);
            }
            if (flag3)
            {
                writer.Write(num5);
            }
        }
    }
}
