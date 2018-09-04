using System.IO;
using Pegasus.Network.Packet.Update.Structure;

namespace Pegasus.Network.Packet.Update
{
    [UpdatePacket(UpdateType.Initial)]
    public class ClientUpdateInitial : ClientUpdatePacket
    {
        public InitialUpdateStructure InitialUpdate { get; private set; }
        public Class6 Class6 { get; } = new Class6();

        public override void ReadUpdate(BinaryReader reader)
        {
            InitialUpdate = reader.ReadStruct<InitialUpdateStructure>();
            Class6.Read(reader);
        }
    }
}
