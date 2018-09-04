using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;

namespace Pegasus.DungeonGenerator.Datastore.Container
{
    public class ReadOnlyHashTable<T> : IUnpackable where T : IUnpackable, new()
    {
        public ImmutableDictionary<uint, T> Buckets { get; private set; }

        public void UnPack(BinaryReader reader)
        {
            var buckets = new Dictionary<uint, T>();

            reader.ReadByte();

            byte entryCount = reader.ReadByte();
            for (byte i = 0; i < entryCount; i++)
            {
                var entry = new T();
                entry.UnPack(reader);
                buckets.Add(reader.ReadUInt32(), entry);
            }

            Buckets = buckets.ToImmutableDictionary();
        }
    }
}
