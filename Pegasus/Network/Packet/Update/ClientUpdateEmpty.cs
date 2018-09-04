using System.IO;

namespace Pegasus.Network.Packet.Update
{
    [UpdatePacket(UpdateType.Stop)]
    [UpdatePacket(UpdateType.ForceBuff)]
    [UpdatePacket(UpdateType.ForceBuffCancel)]
    public class ClientUpdateEmpty : ClientUpdatePacket
    {
        public override void ReadUpdate(BinaryReader reader)
        {
        }
    }
}
