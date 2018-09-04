using System.IO;

namespace Pegasus.Network.Packet.Update
{
    [UpdatePacket(UpdateType.SpellCastAttempt)]
    public class ClientUpdateSpellCastAttempt : ClientUpdatePacket
    {
        public uint SpellId { get; private set; }
        public uint Target { get; private set; }
        public uint Skill { get; private set; }

        public override void ReadUpdate(BinaryReader reader)
        {
            SpellId = reader.ReadUInt32();
            Target  = reader.ReadUInt32();
            Skill   = reader.ReadPackedUInt32();
        }
    }
}
