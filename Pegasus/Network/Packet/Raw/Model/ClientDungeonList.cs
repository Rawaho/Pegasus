using System.IO;

namespace Pegasus.Network.Packet.Raw.Model
{
    [ClientRawPacket(ClientRawOpcode.DungeonList)]
    public class ClientDungeonList : IReadable
    {
        public string SearchParameter { get; private set; }

        public void Read(BinaryReader reader)
        {
            SearchParameter = reader.ReadPackedString();
        }
    }
}
