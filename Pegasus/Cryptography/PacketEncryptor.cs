using System;
using System.IO;
using Pegasus.Network;

namespace Pegasus.Cryptography
{
    public class PacketEncryptor
    {
        private Class121 class121_0 = new Class121();

        public byte[] Encrypt(byte[] data)
        {
            byte[] buffer2;
            try
            {
                using (MemoryStream stream = new MemoryStream())
                {
                    using (BitStream bitStream = new BitStream(stream, BitStreamMode.Write))
                    {
                        bitStream.method_1(false);
                        for (int i = 0; i < data.Length; i++)
                        {
                            Class122 class2 = this.class121_0.method_2(data[i]);
                            if (class2 == null)
                            {
                                class2 = this.class121_0.method_3();
                            }

                            int[] numArray = this.class121_0.method_1(class2);
                            bitStream.method_3(numArray, 0, numArray.Length);
                            if (class2.method_6() == Enum29.const_0)
                            {
                                bitStream.WriteByte(data[i]);
                            }

                            this.class121_0.method_0(data[i]);
                        }

                        int[] numArray2 = this.class121_0.method_1(this.class121_0.method_4());
                        bitStream.method_3(numArray2, 0, numArray2.Length);
                        bitStream.Close();
                        byte[] buffer = new byte[(int)stream.Length];
                        stream.Position = 0L;
                        stream.Read(buffer, 0, buffer.Length);
                        bitStream.Dispose();
                        stream.Dispose();
                        buffer2 = buffer;
                    }
                }
            }
            catch (Exception exception)
            {
                throw exception;
            }

            return buffer2;
        }
    }
}
