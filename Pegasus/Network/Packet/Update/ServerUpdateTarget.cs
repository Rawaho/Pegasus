using System.IO;

namespace Pegasus.Network.Packet.Update
{
    public class ServerUpdateTarget : ServerUpdatePacket
    {
        public uint Target { get; set; }

        public ServerUpdateTarget(uint sequence)
            : base(sequence, UpdateType.Target)
        {
        }

        public override void WriteUpdate(BinaryWriter writer)
        {
            writer.Write(Target);
        }
    }
}
