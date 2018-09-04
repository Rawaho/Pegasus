using System.IO;
using Pegasus.Network.Packet.Raw;

namespace Pegasus.Network.Packet.Update
{
    public abstract class ServerUpdatePacket : ServerRawPacket
    {
        private readonly uint sequence;
        private readonly UpdateType updateType;
        private readonly UpdateFlag updateFlags;

        protected ServerUpdatePacket(uint sequence, UpdateType updateType, UpdateFlag updateFlags = UpdateFlag.None)
            : base(ServerRawOpcode.Update)
        {
            this.sequence    = sequence;
            this.updateType  = updateType;
            this.updateFlags = updateFlags;
        }

        public override void Write(BinaryWriter writer)
        {
            writer.WritePackedUInt32(sequence);
            writer.WritePackedUInt32((uint)updateFlags);
            writer.WritePackedUInt32((uint)updateType);
            WriteUpdate(writer);
        }

        public abstract void WriteUpdate(BinaryWriter writer);
    }
}
