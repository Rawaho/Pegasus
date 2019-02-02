using System;
using System.IO;
using System.Runtime.InteropServices;
using Pegasus.Network.Packet.Object;

namespace Pegasus.Network
{
    public class WrappedStruct<T> : Class4 where T : struct 
    {
        public T Value { get; set; }

        public WrappedStruct()
        {
        }

        public WrappedStruct(T value)
        {
            this.Value = value;
        }

        public override void Read(BinaryReader reader)
        {
            int count = Marshal.SizeOf(typeof(T));
            byte[] buffer = new byte[count];
            reader.Read(buffer, 0, count);
            IntPtr zero = IntPtr.Zero;
            try
            {
                zero = Marshal.AllocHGlobal(count);
                for (int i = 0; i < count; i++)
                {
                    Marshal.WriteByte(zero, i, buffer[i]);
                }
                this.Value = (T)Marshal.PtrToStructure(zero, typeof(T));
            }
            finally
            {
                Marshal.FreeHGlobal(zero);
            }
        }

        public override void Write(BinaryWriter writer)
        {
            int cb = Marshal.SizeOf(typeof(T));
            byte[] buffer = new byte[cb];
            IntPtr zero = IntPtr.Zero;
            try
            {
                zero = Marshal.AllocHGlobal(cb);
                Marshal.StructureToPtr(this.Value, zero, false);
                for (int i = 0; i < cb; i++)
                {
                    buffer[i] = Marshal.ReadByte(zero, i);
                }
            }
            finally
            {
                Marshal.FreeHGlobal(zero);
            }
            writer.Write(buffer, 0, cb);
        }
    }
}
