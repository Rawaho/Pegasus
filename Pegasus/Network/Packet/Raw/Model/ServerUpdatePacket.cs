using System.IO;
using Pegasus.Network.Packet.Update;

namespace Pegasus.Network.Packet.Raw.Model
{
    [ServerRawMessage(ServerRawOpcode.Update)]
    public class ServerUpdatePacket : IWritable
    {
        public uint Sequence { get; set; }
        public UpdateFlag UpdateFlags { get; set; }
        public UpdateType UpdateType { get; set; }
        public IWritable Update { get; set; }

        public void Write(BinaryWriter writer)
        {
            writer.WritePackedUInt32(Sequence);
            writer.WritePackedUInt32((uint)UpdateFlags);
            writer.WritePackedUInt32((uint)UpdateType);
            Update.Write(writer);
        }
    }
}
