using System.IO;

namespace Pegasus.Network.Packet.Update.Model
{
    [UpdateMessage(UpdateType.AllSpellsExpired)]
    public class UpdateAllSpellsExpired : IReadable, IWritable
    {
        public uint SpellId { get; set; }
        public uint Target { get; set; }

        public void Read(BinaryReader reader)
        {
            SpellId = reader.ReadUInt32();
            Target  = reader.ReadUInt32();
        }

        public void Write(BinaryWriter writer)
        {
            writer.Write(SpellId);
            writer.Write(Target);
        }
    }
}
