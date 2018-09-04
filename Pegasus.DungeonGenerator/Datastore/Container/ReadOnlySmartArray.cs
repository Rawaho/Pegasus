using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;

namespace Pegasus.DungeonGenerator.Datastore.Container
{
    public class ReadOnlySmartArray<T> : IUnpackable where T : IUnpackable, new()
    {
        public ImmutableHashSet<T> Entries { get; private set; }

        public void UnPack(BinaryReader reader)
        {
            var entries = new HashSet<T>();

            byte entryCount = reader.ReadByte();
            for (byte i = 0; i < entryCount; i++)
            {
                var entry = new T();
                entry.UnPack(reader);
                entries.Add(entry);
            }

            Entries = entries.ToImmutableHashSet();
        }
    }
}
