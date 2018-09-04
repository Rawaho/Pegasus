using System.IO;

namespace Pegasus.Network.Packet.Update
{
    [UpdatePacket(UpdateType.Use)]
    public class ClientUpdateUse : ClientUpdatePacket
    {
        public int Guid { get; private set; }

        public override void ReadUpdate(BinaryReader reader)
        {
            Guid = reader.ReadInt32();
        }
    }
}
