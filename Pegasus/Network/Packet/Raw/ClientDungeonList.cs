using System.IO;

namespace Pegasus.Network.Packet.Raw
{
    [ClientRawPacket(ClientRawOpcode.DungeonList)]
    public class ClientDungeonList : ClientRawPacket
    {
        public string SearchParameter { get; private set; }

        public override void Read(BinaryReader reader)
        {
            SearchParameter = reader.ReadPackedString();
        }
    }
}
