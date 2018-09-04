using System.IO;

namespace Pegasus.Network.Packet.Update
{
    [UpdatePacket(UpdateType.Health)]
    [UpdatePacket(UpdateType.Stamina)]
    [UpdatePacket(UpdateType.Mana)]
    public class ClientUpdateVital : ClientUpdatePacket
    {
        public uint Maximum { get; private set; }
        public uint Current { get; private set; }

        public override void ReadUpdate(BinaryReader reader)
        {
            Maximum = reader.ReadPackedUInt32();
            Current = reader.ReadPackedUInt32();
            reader.ReadPackedUInt32();
        }
    }
}
