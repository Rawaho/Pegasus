using System;
using System.IO;

namespace Pegasus.Network.Packet
{
    public class FragmentedBuffer
    {
        public bool IsComplete => data.Length == offset;

        public byte[] Data
        {
            get
            {
                if (!IsComplete)
                    throw new InvalidOperationException();
                return data;
            }
        }

        private int offset;
        private readonly byte[] data;

        public FragmentedBuffer(uint size)
        {
            data = new byte[size];
        }

        public void Populate(BinaryReader reader)
        {
            if (IsComplete && data.Length != 0)
                throw new InvalidOperationException();

            int remaining = (int)(reader.BaseStream.Length - reader.BaseStream.Position);
            if (remaining < data.Length - offset)
            {
                // not enough data, push entire frame into packet
                byte[] newData = reader.ReadBytes(remaining);
                Buffer.BlockCopy(newData, 0, data, offset, remaining);

                offset += newData.Length;
            }
            else
            {
                // enough data, push required frame into packet
                byte[] newData = reader.ReadBytes(data.Length - offset);
                Buffer.BlockCopy(newData, 0, data, offset, newData.Length);

                offset += newData.Length;
            }
        }
    }
}
