using System.IO;

namespace Pegasus.Network.Packet.Update
{
    public class ServerUpdateVital : ServerUpdatePacket
    {
        public uint Current { get; set; }
        public uint Maximum { get; set; }

        public ServerUpdateVital(uint sequence, UpdateType updateType)
            : base(sequence, updateType)
        {
        }

        public override void WriteUpdate(BinaryWriter writer)
        {
            writer.WritePackedUInt32(0);
            writer.WritePackedUInt32(Current);
            writer.WritePackedUInt32(Maximum);
        }
    }
}
