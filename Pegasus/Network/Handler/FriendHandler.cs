using NLog;
using Pegasus.Network.Packet.Object;
using Pegasus.Social;

namespace Pegasus.Network.Handler
{
    public static class FriendHandler
    {
        private static readonly Logger log = LogManager.GetCurrentClassLogger();

        [ObjectPacketHandler(ObjectOpcode.FriendList)]
        public static void HandleFriendList(Session session, NetworkObject networkObject)
        {
            ClientFriendAction action = (ClientFriendAction)NetworkObjectField.ReadIntField(networkObject.GetField(0));
            switch (action)
            {
                case ClientFriendAction.Create:
                    session.FriendManager.AddAccountAll(NetworkObjectField.ReadStringField(networkObject.GetField(1)));
                    break;
                case ClientFriendAction.Remove:
                    session.FriendManager.RemoveAccountAll(NetworkObjectField.ReadStringField(networkObject.GetField(1)));
                    break;
                default:
                    log.Warn($"[{session.Account.Username}] Invalid FriendAction {action}!");
                    break;
            }
        }
    }
}
