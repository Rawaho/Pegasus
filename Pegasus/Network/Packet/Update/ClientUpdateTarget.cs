using System.IO;

namespace Pegasus.Network.Packet.Update
{
    [UpdatePacket(UpdateType.Target)]
    public class ClientUpdateTarget : ClientUpdatePacket
    {
        public uint Target { get; private set; }

        public override void ReadUpdate(BinaryReader reader)
        {
            Target = reader.ReadUInt32();
        }
    }
}
