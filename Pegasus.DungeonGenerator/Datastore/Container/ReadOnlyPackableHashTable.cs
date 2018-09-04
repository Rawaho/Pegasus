using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.IO;

namespace Pegasus.DungeonGenerator.Datastore.Container
{
    public class ReadOnlyPackableHashTable<T> : IUnpackable where T : IUnpackable, new()
    {
        public ImmutableDictionary<uint, T> Buckets { get; private set; }

        public void UnPack(BinaryReader reader)
        {
            uint tableEntries = reader.ReadUInt16();
            uint tableSize    = reader.ReadUInt16();
            Debug.Assert(tableSize <= 0x010000);

            var buckets = new Dictionary<uint, T>();
            for (uint i = 0; i < tableEntries; i++)
            {
                uint key = reader.ReadUInt32();

                var entry = new T();
                entry.UnPack(reader);
                buckets.Add(key, entry);
            }

            Buckets = buckets.ToImmutableDictionary();
        }
    }
}
