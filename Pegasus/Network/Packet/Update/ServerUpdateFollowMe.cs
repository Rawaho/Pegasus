using System.IO;

namespace Pegasus.Network.Packet.Update
{
    public class ServerUpdateFollowMe : ServerUpdatePacket
    {
        public int Guid { get; set; }
        public string Ftn { get; set; }

        public ServerUpdateFollowMe()
            : base(0u, UpdateType.FollowMe, UpdateFlag.flag_1)
        {
        }

        public override void WriteUpdate(BinaryWriter writer)
        {
            writer.Write(Guid);
            writer.WritePackedString(Ftn);
        }
    }
}
