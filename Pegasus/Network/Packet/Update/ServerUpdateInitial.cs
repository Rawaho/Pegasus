using System.IO;
using Pegasus.Network.Packet.Update.Structure;

namespace Pegasus.Network.Packet.Update
{
    public class ServerUpdateInitial : ServerUpdatePacket
    {
        public InitialUpdateStructure InitialUpdate { get; set; }
        public Class6 Class6 { get; set; }

        public ServerUpdateInitial(uint sequence)
            : base(sequence, UpdateType.Initial)
        {
        }

        public override void WriteUpdate(BinaryWriter writer)
        {
            writer.WriteStruct(InitialUpdate);
            Class6.Write(writer);
        }
    }
}
