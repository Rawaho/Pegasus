using System;

namespace Pegasus.DungeonGenerator.Datastore
{
    public class DatastoreFileNotFoundException : Exception
    {
        public DatastoreFileNotFoundException(uint full)
            : base($"File 0x{full:X8} doesn't exist in the datastore!")
        {
        }
    }
}
