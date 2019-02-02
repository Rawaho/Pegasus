using System.IO;

namespace Pegasus.Network.Packet.Update.Model
{
    [UpdateMessage(UpdateType.SpellCastComplete)]
    public class UpdateSpellCastComplete : IReadable, IWritable
    {
        public uint SpellId { get; private set; }
        public uint Target { get; private set; }
        public uint Duration { get; private set; }

        public void Read(BinaryReader reader)
        {
            SpellId  = reader.ReadUInt32();
            Target   = reader.ReadUInt32();
            Duration = reader.ReadUInt32();
        }

        public void Write(BinaryWriter writer)
        {
            writer.Write(SpellId);
            writer.Write(Target);
            writer.Write(Duration);
        }
    }
}
