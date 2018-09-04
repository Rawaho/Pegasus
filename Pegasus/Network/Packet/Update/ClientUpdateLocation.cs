using System.IO;
using Pegasus.Network.Packet.Update.Structure;

namespace Pegasus.Network.Packet.Update
{
    [UpdatePacket(UpdateType.Location)]
    public class ClientUpdateLocation : ClientUpdatePacket
    {
        public WorldLocationStructure Location { get; private set; }
        public uint CellId { get; private set; }

        public override void ReadUpdate(BinaryReader reader)
        {
            Location = reader.ReadStruct<WorldLocationStructure>();
            CellId   = reader.ReadUInt32();
        }
    }
}
