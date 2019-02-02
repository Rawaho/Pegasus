using Pegasus.Network.Packet;
using Pegasus.Network.Packet.Raw;
using Pegasus.Network.Packet.Raw.Model;

namespace Pegasus.Network.Handler
{
    public static class MiscellaneousHandler
    {
        [RawMessageHandler(ClientRawOpcode.CompList)]
        public static void HandleCompList(Session session, ClientCompList compList)
        {
        }
    }
}
