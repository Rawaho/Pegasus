using System.IO;

namespace Pegasus.Network.Packet.Update
{
    public class ServerUpdateAllSpellsExpired : ServerUpdatePacket
    {
        public uint SpellId { get; set; }
        public uint Target { get; set; }

        public ServerUpdateAllSpellsExpired(uint sequence)
            : base(sequence, UpdateType.AllSpellsExpired)
        {
        }

        public override void WriteUpdate(BinaryWriter writer)
        {
            writer.Write(SpellId);
            writer.Write(Target);
        }
    }
}
