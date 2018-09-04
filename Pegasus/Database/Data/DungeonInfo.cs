using System.Data;

namespace Pegasus.Database.Data
{
    public class DungeonInfo : IReadable
    {
        public ushort LandBlockId { get; private set; }
        public string Name { get; private set; }

        public void Read(DataRow row)
        {
            LandBlockId = row.Read<ushort>("landBlockId");
            Name        = row.Read<string>("name");
        }
    }
}
