using System;
using System.IO;

namespace Pegasus.Network
{
    public class BitStream : Stream, IDisposable
    {
        private bool bool_0;
        private bool bool_1;
        private byte byte_0;
        private BitStreamMode mode;
        private int int_0;
        private long position;
        private Stream stream;

        public BitStream(Stream stream_1, BitStreamMode bitStreamMode1)
        {
            this.mode = bitStreamMode1;
            this.stream = stream_1;
            if (!((bitStreamMode1 != BitStreamMode.Read) || stream_1.CanRead))
            {
                throw new ArgumentException("Can't read from the underlying stream.");
            }
            if (!((bitStreamMode1 != BitStreamMode.Write) || stream_1.CanWrite))
            {
                throw new ArgumentException("Can't write to the underlying stream.");
            }
            this.byte_0 = 0;
            this.int_0 = 0;
            this.position = 0L;
            this.bool_0 = true;
            this.bool_1 = true;
        }

        public override IAsyncResult BeginRead(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
        {
            throw new NotSupportedException("Can't asynchronously access a bit stream.");
        }

        public override IAsyncResult BeginWrite(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
        {
            throw new NotSupportedException("Can't asynchronously access a bit stream.");
        }

        public override void Close()
        {
            if (this.bool_0)
            {
                if (this.CanWrite && (this.int_0 != 0))
                {
                    this.stream.WriteByte(this.byte_0);
                }
                if (this.method_0())
                {
                    this.stream.Close();
                }
                this.stream = null;
                this.byte_0 = 0;
                this.int_0 = 0;
                this.bool_0 = false;
            }
        }

        public override int EndRead(IAsyncResult asyncResult)
        {
            throw new NotSupportedException("Can't asynchronously access a bit stream.");
        }

        public override void EndWrite(IAsyncResult asyncResult)
        {
            throw new NotSupportedException("Can't asynchronously access a bit stream.");
        }

        ~BitStream()
        {
            this.Close();
        }

        public override void Flush()
        {
            this.stream.Flush();
        }

        public bool method_0()
        {
            return this.bool_1;
        }

        public void method_1(bool bool_2)
        {
            this.bool_1 = bool_2;
        }

        public void method_2(int int_1)
        {
            if (!this.CanWrite)
            {
                throw new NotSupportedException("Can't write on this stream.");
            }
            if ((int_1 != 0) && (int_1 != 1))
            {
                throw new ArgumentException("The bit must be 0 or 1.", "bit");
            }
            this.byte_0 = (byte)(this.byte_0 | (int_1 << (7 - this.int_0)));
            if (++this.int_0 == 8)
            {
                this.stream.WriteByte(this.byte_0);
                this.byte_0 = 0;
                this.int_0 = 0;
            }
            this.position += 1L;
        }

        public void method_3(int[] int_1, int int_2, int int_3)
        {
            for (int i = int_2; i < (int_2 + int_3); i++)
            {
                this.method_2(int_1[i]);
            }
        }

        public int method_4()
        {
            if (!this.CanRead)
            {
                throw new NotSupportedException("Can't read on this stream.");
            }
            if (this.int_0 == 0)
            {
                int num2 = this.stream.ReadByte();
                if (num2 == -1)
                {
                    this.byte_0 = 0;
                    return -1;
                }
                this.byte_0 = (byte)num2;
            }
            int num = (((((int)1) << (7 - this.int_0)) & this.byte_0) > 0) ? 1 : 0;
            this.int_0 = (this.int_0 + 1) % 8;
            this.position += 1L;
            return num;
        }

        public int method_5(int[] int_1, int int_2, int int_3)
        {
            for (int i = int_2; i < (int_2 + int_3); i++)
            {
                int num2 = this.method_4();
                if (num2 >= 0)
                {
                    int_1[i] = num2;
                }
                else
                {
                    return (i - int_2);
                }
            }
            return int_3;
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            for (int i = offset; i < (offset + count); i++)
            {
                int num2 = this.ReadByte();
                if (num2 >= 0)
                {
                    buffer[i] = (byte)num2;
                }
                else
                {
                    return (i - offset);
                }
            }
            return count;
        }

        public override int ReadByte()
        {
            byte num = 0;
            for (int i = 0; i < 8; i++)
            {
                int num3 = this.method_4();
                if (num3 < 0)
                {
                    return num3;
                }
                num = (byte)(num | (num3 << (7 - i)));
            }
            return num;
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new NotSupportedException("Can't seek on a bit stream.");
        }

        public override void SetLength(long value)
        {
            throw new NotSupportedException("Can't set the length of a BitStream.");
        }

        void IDisposable.Dispose()
        {
            this.Close();
            GC.SuppressFinalize(this);
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            for (int i = offset; i < (offset + count); i++)
            {
                this.WriteByte(buffer[i]);
            }
        }

        public override void WriteByte(byte value)
        {
            for (int i = 0; i < 8; i++)
            {
                int num2 = (((((int)1) << (7 - i)) & value) > 0) ? 1 : 0;
                this.method_2(num2);
            }
        }

        public override bool CanRead => (this.bool_0 && (this.mode == BitStreamMode.Read));

        public override bool CanSeek => false;

        public override bool CanWrite => (this.bool_0 && (this.mode == BitStreamMode.Write));

        public override long Length => throw new NotSupportedException("Can't seek on a bit stream.");

        public override long Position
        {
            get => position;
            set => throw new NotSupportedException("Can't seek on a bit stream.");
        }
    }
}
