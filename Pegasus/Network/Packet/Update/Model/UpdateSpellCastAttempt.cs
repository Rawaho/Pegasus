using System.IO;

namespace Pegasus.Network.Packet.Update.Model
{
    [UpdateMessage(UpdateType.SpellCastAttempt)]
    public class UpdateSpellCastAttempt : IReadable, IWritable
    {
        public uint SpellId { get; set; }
        public uint Target { get; set; }
        public uint Skill { get; set; }

        public void Read(BinaryReader reader)
        {
            SpellId = reader.ReadUInt32();
            Target  = reader.ReadUInt32();
            Skill   = reader.ReadPackedUInt32();
        }

        public void Write(BinaryWriter writer)
        {
            writer.Write(SpellId);
            writer.Write(Target);
            writer.WritePackedUInt32(Skill);
        }
    }
}
