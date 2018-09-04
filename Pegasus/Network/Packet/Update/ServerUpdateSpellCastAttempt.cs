using System.IO;

namespace Pegasus.Network.Packet.Update
{
    public class ServerUpdateSpellCastAttempt : ServerUpdatePacket
    {
        public uint SpellId { get; set; }
        public uint Target { get; set; }
        public uint Skill { get; set; }

        public ServerUpdateSpellCastAttempt(uint sequence)
            : base(sequence, UpdateType.SpellCastAttempt)
        {
        }

        public override void WriteUpdate(BinaryWriter writer)
        {
            writer.Write(SpellId);
            writer.Write(Target);
            writer.WritePackedUInt32(Skill);
        }
    }
}
