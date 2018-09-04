using System;
using System.Diagnostics;
using System.IO;
using Pegasus.Network;

namespace Pegasus.Cryptography
{
    public class PacketDecryptor
    {
        private Class121 class121_0 = new Class121();

        public byte[] Decrypt(byte[] byte_0)
        {
            byte[] buffer2;
            try
            {
                MemoryStream stream = new MemoryStream();
                MemoryStream stream2 = new MemoryStream();
                BitStream stream3 = new BitStream(stream, BitStreamMode.Read);
                stream.Write(byte_0, 0, byte_0.Length);
                stream.Position = 0L;
                byte num = 0;
                while (this.method_0(stream3, out num))
                {
                    stream2.WriteByte(num);
                }
                byte[] buffer = new byte[(int)stream2.Length];
                stream2.Position = 0L;
                stream2.Read(buffer, 0, buffer.Length);
                stream3.Dispose();
                stream.Dispose();
                stream2.Dispose();
                buffer2 = buffer;
            }
            catch (Exception exception)
            {
                Debug.Print(exception.ToString());
                throw exception;
            }
            return buffer2;
        }

        private bool method_0(BitStream stream0_0, out byte byte_0)
        {
            Class122 class2 = this.class121_0.method_9();
            if (class2 == null)
            {
                throw new ApplicationException("The tree cannot be empty.");
            }
            while (class2.method_8() != null)
            {
                int num = stream0_0.method_4();
                if (num < 0)
                {
                    byte_0 = 0;
                    return false;
                }
                if (num != 0)
                {
                    if (num != 1)
                    {
                        throw new ApplicationException("Invalid bit in the stream.");
                    }
                    class2 = class2.method_10();
                }
                else
                {
                    class2 = class2.method_8();
                    continue;
                }
            }
            if (class2.method_6() == Enum29.const_1)
            {
                byte_0 = 0;
                return false;
            }
            int num2 = (class2.method_6() == Enum29.const_0) ? stream0_0.ReadByte() : class2.method_4();
            if (num2 < 0)
            {
                byte_0 = 0;
                return false;
            }
            this.class121_0.method_0((byte)num2);
            byte_0 = (byte)num2;
            return true;
        }
    }
}
