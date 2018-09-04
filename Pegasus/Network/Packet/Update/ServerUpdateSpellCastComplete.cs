using System.IO;

namespace Pegasus.Network.Packet.Update
{
    public class ServerUpdateSpellCastComplete : ServerUpdatePacket
    {
        public uint SpellId { get; set; }
        public uint Target { get; set; }
        public uint Duration { get; set; }

        public ServerUpdateSpellCastComplete(uint sequence)
            : base(sequence, UpdateType.SpellCastComplete)
        {
        }

        public override void WriteUpdate(BinaryWriter writer)
        {
            writer.Write(SpellId);
            writer.Write(Target);
            writer.Write(Duration);
        }
    }
}
