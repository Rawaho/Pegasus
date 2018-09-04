using System.IO;

namespace Pegasus.Network.Packet.Update
{
    [UpdatePacket(UpdateType.FollowMe)]
    public class ClientUpdateFollowMe : ClientUpdatePacket
    {
        public int Guid { get; private set; }
        public string Ftn { get; private set; }

        public override void ReadUpdate(BinaryReader reader)
        {
            Guid = reader.ReadInt32();
            Ftn  = reader.ReadPackedString();
        }
    }
}
