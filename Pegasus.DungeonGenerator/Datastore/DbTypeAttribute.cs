using System;

namespace Pegasus.DungeonGenerator.Datastore
{
    [AttributeUsage(AttributeTargets.Field)]
    public class DbTypeAttribute : Attribute
    {
        public uint Base { get; }
        public uint Top { get; }

        public DbTypeAttribute(uint baseId, uint topId)
        {
            Base = baseId;
            Top  = topId;
        }
    }
}
