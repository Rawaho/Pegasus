using System.IO;
using Pegasus.Network.Packet.Update.Structure;

namespace Pegasus.Network.Packet.Update.Model
{
    [UpdateMessage(UpdateType.Location)]
    public class UpdateLocation : IReadable, IWritable
    {
        public WorldLocationStructure Location { get; set; }
        public uint CellId { get; set; }

        public void Read(BinaryReader reader)
        {
            Location = reader.ReadStruct<WorldLocationStructure>();
            CellId   = reader.ReadUInt32();
        }

        public void Write(BinaryWriter writer)
        {
            writer.WriteStruct(Location);
            writer.Write(CellId);
        }
    }
}
