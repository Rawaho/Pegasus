using System.IO;

namespace Pegasus.Network.Packet.Update
{
    public class ServerUpdateUse : ServerUpdatePacket
    {
        public int Guid { get; set; }

        public ServerUpdateUse()
            : base(0u, UpdateType.Use, UpdateFlag.flag_1)
        {
        }

        public override void WriteUpdate(BinaryWriter writer)
        {
            writer.Write(Guid);
        }
    }
}
