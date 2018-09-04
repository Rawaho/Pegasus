using System.IO;

namespace Pegasus.Network.Packet.Update
{
    public class ServerUpdateEmpty : ServerUpdatePacket
    {
        public ServerUpdateEmpty(uint sequence, UpdateType updateType, UpdateFlag updateFlags = UpdateFlag.None)
            : base(sequence, updateType, updateFlags)
        {
        }

        public override void WriteUpdate(BinaryWriter writer)
        {
        }
    }
}
