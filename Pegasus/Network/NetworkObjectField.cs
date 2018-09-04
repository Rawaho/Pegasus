using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Pegasus.Network
{
    public class NetworkObjectField
    {
        private bool isNetworkObject;
        private byte[] byte_0;
        private NetworkObject class63_0;

        public NetworkObjectField()
        {
            this.byte_0 = null;
            this.class63_0 = null;
            this.isNetworkObject = false;
            this.byte_0 = new byte[0];
        }

        public NetworkObjectField(byte[] byte_1)
        {
            this.byte_0 = null;
            this.class63_0 = null;
            this.isNetworkObject = false;
            this.byte_0 = byte_1;
        }

        public NetworkObjectField(NetworkObject class63_1)
        {
            this.byte_0 = null;
            this.class63_0 = null;
            this.isNetworkObject = false;
            this.isNetworkObject = true;
            this.class63_0 = class63_1;
        }

        public NetworkObjectField(bool bool_1)
        {
            this.byte_0 = null;
            this.class63_0 = null;
            this.isNetworkObject = false;
            this.byte_0 = new byte[1];
            if (bool_1)
            {
                this.byte_0[0] = 1;
            }
            else
            {
                this.byte_0[0] = 0;
            }
        }

        public NetworkObjectField(byte byte_1)
        {
            this.byte_0 = null;
            this.class63_0 = null;
            this.isNetworkObject = false;
            this.byte_0 = new byte[] { byte_1 };
        }

        public NetworkObjectField(double double_0)
        {
            this.byte_0 = null;
            this.class63_0 = null;
            this.isNetworkObject = false;
            this.byte_0 = BitConverter.GetBytes(double_0);
        }

        public NetworkObjectField(short short_0)
        {
            this.byte_0 = null;
            this.class63_0 = null;
            this.isNetworkObject = false;
            this.byte_0 = BitConverter.GetBytes(short_0);
        }

        public NetworkObjectField(int int_0)
        {
            this.byte_0 = null;
            this.class63_0 = null;
            this.isNetworkObject = false;
            this.byte_0 = BitConverter.GetBytes(int_0);
        }

        public NetworkObjectField(long long_0)
        {
            this.byte_0 = null;
            this.class63_0 = null;
            this.isNetworkObject = false;
            this.byte_0 = BitConverter.GetBytes(long_0);
        }

        public NetworkObjectField(float float_0)
        {
            this.byte_0 = null;
            this.class63_0 = null;
            this.isNetworkObject = false;
            this.byte_0 = BitConverter.GetBytes(float_0);
        }

        public NetworkObjectField(string string_0)
        {
            this.byte_0 = null;
            this.class63_0 = null;
            this.isNetworkObject = false;
            this.byte_0 = new ASCIIEncoding().GetBytes(string_0);
        }

        public NetworkObjectField(ushort ushort_0)
        {
            this.byte_0 = null;
            this.class63_0 = null;
            this.isNetworkObject = false;
            this.byte_0 = BitConverter.GetBytes(ushort_0);
        }

        public static NetworkObjectField CreateFromCustom<T>(T gparam_0) where T : struct
        {
            int cb = Marshal.SizeOf(typeof(T));
            byte[] buffer = new byte[cb];
            IntPtr zero = IntPtr.Zero;
            try
            {
                zero = Marshal.AllocHGlobal(cb);
                Marshal.StructureToPtr(gparam_0, zero, false);
                for (int i = 0; i < cb; i++)
                {
                    buffer[i] = Marshal.ReadByte(zero, i);
                }
            }
            finally
            {
                Marshal.FreeHGlobal(zero);
            }
            return new NetworkObjectField { byte_0 = buffer };
        }

        public T GetCustom<T>() where T : struct
        {
            T local;
            if (this.isNetworkObject)
            {
                throw new Exception("Attempted to access data in a struct field.");
            }
            int cb = Marshal.SizeOf(typeof(T));
            if (cb > this.byte_0.Length)
            {
                throw new Exception("Insufficient data to decode type.");
            }
            IntPtr zero = IntPtr.Zero;
            try
            {
                zero = Marshal.AllocHGlobal(cb);
                for (int i = 0; i < cb; i++)
                {
                    Marshal.WriteByte(zero, i, this.byte_0[i]);
                }
                local = (T)Marshal.PtrToStructure(zero, typeof(T));
            }
            finally
            {
                Marshal.FreeHGlobal(zero);
            }
            return local;
        }

        public bool IsNetworkObject()
        {
            return this.isNetworkObject;
        }

        public byte[] method_1()
        {
            if (this.isNetworkObject)
            {
                return NetworkObject.Pack(this.class63_0);
            }
            return this.byte_0;
        }

        public NetworkObject ReadObject()
        {
            if (!this.isNetworkObject)
            {
                throw new Exception("Attempted to access struct in a data field.");
            }
            return this.class63_0;
        }

        public NetworkObjectField method_3(int int_0)
        {
            if (!this.isNetworkObject)
            {
                throw new Exception("Attempted to access struct in a data field.");
            }
            return this.class63_0.GetField(int_0);
        }

        public static NetworkObjectField CreateObjectField(NetworkObject class63_1)
        {
            return new NetworkObjectField(class63_1);
        }

        public static byte[] smethod_1(NetworkObjectField class62_0)
        {
            if (class62_0.isNetworkObject)
            {
                throw new Exception("Attempted to access data in a struct field.");
            }
            return class62_0.byte_0;
        }

        public static bool smethod_10(NetworkObjectField class62_0)
        {
            if (class62_0.isNetworkObject)
            {
                throw new Exception("Attempted to access data in a struct field.");
            }
            return (class62_0.byte_0[0] != 0);
        }

        public static NetworkObjectField CreateByteArrayField(byte[] byte_1)
        {
            return new NetworkObjectField(byte_1);
        }

        public static NetworkObjectField CreateStringField(string string_0)
        {
            return new NetworkObjectField(string_0);
        }

        public static NetworkObjectField CreateByteField(byte byte_1)
        {
            return new NetworkObjectField(byte_1);
        }

        public static NetworkObjectField CreateUShortField(ushort ushort_0)
        {
            return new NetworkObjectField(ushort_0);
        }

        public static NetworkObjectField CreateShortField(short short_0)
        {
            return new NetworkObjectField(short_0);
        }

        public static NetworkObjectField CreateIntField(int int_0)
        {
            return new NetworkObjectField(int_0);
        }

        public static NetworkObjectField CreateLongField(long long_0)
        {
            return new NetworkObjectField(long_0);
        }

        public static NetworkObjectField CreateFloatField(float float_0)
        {
            return new NetworkObjectField(float_0);
        }

        public static NetworkObjectField CreateDoubleField(double double_0)
        {
            return new NetworkObjectField(double_0);
        }

        public static string ReadStringField(NetworkObjectField class62_0)
        {
            if (class62_0.isNetworkObject)
            {
                throw new Exception("Attempted to access data in a struct field.");
            }
            ASCIIEncoding encoding = new ASCIIEncoding();
            return encoding.GetString(class62_0.byte_0);
        }

        public static NetworkObjectField CreateBoolField(bool bool_1)
        {
            return new NetworkObjectField(bool_1);
        }

        public static byte ReadByteField(NetworkObjectField class62_0)
        {
            if (class62_0.isNetworkObject)
            {
                throw new Exception("Attempted to access data in a struct field.");
            }
            return class62_0.byte_0[0];
        }

        public static ushort ReadUShortField(NetworkObjectField class62_0)
        {
            if (class62_0.isNetworkObject)
            {
                throw new Exception("Attempted to access data in a struct field.");
            }
            return BitConverter.ToUInt16(class62_0.byte_0, 0);
        }

        public static short ReadShortField(NetworkObjectField class62_0)
        {
            if (class62_0.isNetworkObject)
            {
                throw new Exception("Attempted to access data in a struct field.");
            }
            return BitConverter.ToInt16(class62_0.byte_0, 0);
        }

        public static int ReadIntField(NetworkObjectField class62_0)
        {
            if (class62_0.isNetworkObject)
            {
                throw new Exception("Attempted to access data in a struct field.");
            }
            return BitConverter.ToInt32(class62_0.byte_0, 0);
        }

        public static long ReadLongField(NetworkObjectField class62_0)
        {
            if (class62_0.isNetworkObject)
            {
                throw new Exception("Attempted to access data in a struct field.");
            }
            return BitConverter.ToInt64(class62_0.byte_0, 0);
        }

        public static float ReadFloatField(NetworkObjectField class62_0)
        {
            if (class62_0.isNetworkObject)
            {
                throw new Exception("Attempted to access data in a struct field.");
            }
            return BitConverter.ToSingle(class62_0.byte_0, 0);
        }

        public static double ReadDoubleField(NetworkObjectField class62_0)
        {
            if (class62_0.isNetworkObject)
            {
                throw new Exception("Attempted to access data in a struct field.");
            }
            return BitConverter.ToDouble(class62_0.byte_0, 0);
        }
    }
}
