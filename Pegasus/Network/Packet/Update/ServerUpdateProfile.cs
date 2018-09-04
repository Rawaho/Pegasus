using System.IO;

namespace Pegasus.Network.Packet.Update
{
    public class ServerUpdateProfile : ServerUpdatePacket
    {
        public string Profile { get; set; }

        public ServerUpdateProfile()
            : base(0u, UpdateType.Profile, UpdateFlag.flag_1)
        {
        }

        public override void WriteUpdate(BinaryWriter writer)
        {
            writer.WritePackedString(Profile);
        }
    }
}
