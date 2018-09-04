using Pegasus.Network.Packet;
using Pegasus.Network.Packet.Raw;

namespace Pegasus.Network.Handler
{
    public static class MiscellaneousHandler
    {
        [RawPacketHandler(ClientRawOpcode.CompList)]
        public static void HandleCompList(Session session, ClientCompList compList)
        {
        }
    }
}
