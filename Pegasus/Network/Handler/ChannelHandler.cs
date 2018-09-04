using Pegasus.Network.Packet;
using Pegasus.Social;

namespace Pegasus.Network.Handler
{
    public static class ChannelHandler
    {
        [ObjectPacketHandler(ObjectOpcode.Channel)]
        public static void HandleChannel(Session session, NetworkObject networkObject)
        {
            string channelName = NetworkObjectField.ReadStringField(networkObject.GetField(1));
            if (string.IsNullOrWhiteSpace(channelName))
                return;

            ClientChannelAction action = (ClientChannelAction)NetworkObjectField.ReadIntField(networkObject.GetField(0));
            switch (action)
            {
                case ClientChannelAction.Join:
                {
                    Channel channel = ChannelManager.GetChannel(channelName);
                    channel.AddMember(session);
                    break;
                }
                case ClientChannelAction.Leave:
                {
                    Channel channel = ChannelManager.GetChannel(channelName);
                    channel?.RemoveMember(session);
                    break;
                }
                case ClientChannelAction.Message:
                {
                    Channel channel = ChannelManager.GetChannel(channelName);
                    channel.BroadcastMessage(session, NetworkObjectField.ReadStringField(networkObject.GetField(2)));
                    break;
                }
            }
        }
    }
}
