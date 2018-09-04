using System.IO;

namespace Pegasus.Network.Packet.Update
{
    [UpdatePacket(UpdateType.Profile)]
    public class ClientUpdateProfile : ClientUpdatePacket
    {
        public string Profile { get; private set; }

        public override void ReadUpdate(BinaryReader reader)
        {
            Profile = reader.ReadPackedString();
        }
    }
}
