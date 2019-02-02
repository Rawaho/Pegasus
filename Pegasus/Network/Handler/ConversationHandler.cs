using Pegasus.Network.Packet.Object;
using Pegasus.Social;

namespace Pegasus.Network.Handler
{
    public static class ConversationHandler
    {
        [ObjectPacketHandler(ObjectOpcode.Conversation)]
        public static void HandleConversation(Session session, NetworkObject networkObject)
        {
            NetworkObjectField.ReadIntField(networkObject.GetField(0));
            string recipient = NetworkObjectField.ReadStringField(networkObject.GetField(1));
            string message   = NetworkObjectField.ReadStringField(networkObject.GetField(2));

            ConversationManager.SendMessage(session, recipient, message);
        }
    }
}
