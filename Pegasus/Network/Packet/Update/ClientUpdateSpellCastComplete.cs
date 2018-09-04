using System.IO;

namespace Pegasus.Network.Packet.Update
{
    [UpdatePacket(UpdateType.SpellCastComplete)]
    public class ClientUpdateSpellCastComplete : ClientUpdatePacket
    {
        public uint SpellId { get; private set; }
        public uint Target { get; private set; }
        public uint Duration { get; private set; }

        public override void ReadUpdate(BinaryReader reader)
        {
            SpellId  = reader.ReadUInt32();
            Target   = reader.ReadUInt32();
            Duration = reader.ReadUInt32();
        }
    }
}
