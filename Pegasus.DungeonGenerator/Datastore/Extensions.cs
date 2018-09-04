using System;
using System.IO;
using System.Numerics;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Pegasus.DungeonGenerator.Datastore
{
    public static class Extensions
    {
        public static T DeSerialise<T>(this byte[] data) where T : new()
        {
            GCHandle handle = GCHandle.Alloc(data, GCHandleType.Pinned);
            try
            {
                return (T)Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof(T));
            }
            finally
            {
                handle.Free();
            }
        }

        public static T GetAttributeOfType<T>(this Enum value) where T : Attribute
        {
            MemberInfo[] memberInfo = value.GetType().GetMember(value.ToString());
            object[] attributes = memberInfo[0].GetCustomAttributes(typeof(T), false);
            return (attributes.Length > 0) ? (T)attributes[0] : null;
        }

        public static void Skip(this BinaryReader reader, uint length)
        {
            reader.BaseStream.Position += length;
        }

        private static uint CalculatePadMultiple(uint length, uint multiple)
        {
            return multiple * ((length + multiple - 1u) / multiple) - length;
        }

        public static void Align(this BinaryReader reader)
        {
            reader.Skip(CalculatePadMultiple((uint)reader.BaseStream.Position, 4u));
        }

        public static Vector3 ReadVector3(this BinaryReader reader)
        {
            return new Vector3(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
        }

        public static Quaternion ReadQuaternion(this BinaryReader reader)
        {
            float qw = reader.ReadSingle();
            return new Quaternion(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), qw);
        }
    }
}
