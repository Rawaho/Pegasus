using System;
using System.IO;

namespace Pegasus.Network
{
    // Class42
    public static class Extensions
    {
        public static void WritePackedUInt32(this BinaryWriter writer, uint value)
        {
            new WrappedPackedUInt32
            {
                Value = value
            }.Write(writer);
        }

        public static uint ReadPackedUInt32(this BinaryReader reader)
        {
            WrappedPackedUInt32 class2 = new WrappedPackedUInt32();
            class2.Read(reader);
            return class2.Value;
        }

        public static void WritePackedString(this BinaryWriter writer, string value)
        {
            new WrappedPackedString
            {
                Value = value
            }.Write(writer);
        }

        public static string ReadPackedString(this BinaryReader reader)
        {
            WrappedPackedString class2 = new WrappedPackedString();
            class2.Read(reader);
            return class2.Value;
        }

        public static void WriteStruct<T>(this BinaryWriter writer, T value) where T : struct
        {
            new WrappedStruct<T>
            {
                Value = value
            }.Write(writer);
        }

        public static T ReadStruct<T>(this BinaryReader reader) where T : struct
        {
            var structure = new WrappedStruct<T>();
            structure.Read(reader);
            return structure.Value;
        }

        public static long Remaining(this Stream stream)
        {
            if (stream.Length < stream.Position)
                throw new InvalidOperationException();

            return stream.Length - stream.Position;
        }
    }
}
