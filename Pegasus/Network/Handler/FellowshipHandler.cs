using Pegasus.Network.Packet.Object;
using Pegasus.Social;

namespace Pegasus.Network.Handler
{
    public static class FellowshipHandler
    {
        [ObjectPacketHandler(ObjectOpcode.Fellowship)]
        public static void HandleFellowship(Session session, NetworkObject networkObject)
        {
            FellowshipAction action = (FellowshipAction)NetworkObjectField.ReadIntField(networkObject.GetField(0));
            FellowshipObject fellowshipInfo = new FellowshipObject();
            fellowshipInfo.FromNetworkObject(networkObject.GetField(1).ReadObject());

            switch (action)
            {
                case FellowshipAction.Join:
                {
                    Fellowship fellowship = FellowshipManager.GetFellowship(fellowshipInfo);
                    fellowship.AddMember(session.Character);
                    break;
                }
                case FellowshipAction.Leave:
                {
                    Fellowship fellowship = FellowshipManager.GetFellowship(fellowshipInfo);
                    fellowship?.RemoveMember(session.Character);
                    break;
                }
            }
        }

        [ObjectPacketHandler(ObjectOpcode.const_4)]
        public static void HandleFellowshipMessage(Session session, NetworkObject networkObject)
        {
            int test = NetworkObjectField.ReadIntField(networkObject.GetField(0));
            int tes2t = NetworkObjectField.ReadIntField(networkObject.GetField(1));
        }
    }
}
