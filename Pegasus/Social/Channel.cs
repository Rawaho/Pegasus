using System.Text.RegularExpressions;
using Pegasus.Network;
using Pegasus.Network.Packet.Object;

namespace Pegasus.Social
{
    public class Channel : SocalBase
    {
        private readonly string name;

        public Channel(string name)
        {
            this.name = name;
        }

        public string GetShortcut()
        {
            return Regex.Replace(name.ToLower(), @"\s+", "");
        }

        /// <summary>
        /// 
        /// </summary>
        public override void AddMember(Session member)
        {
            base.AddMember(member);
            member.Channels.Add(this);

            var channelJoin = new NetworkObject();
            channelJoin.AddField(0, NetworkObjectField.CreateIntField((int)ServerChannelAction.Join));
            channelJoin.AddField(1, NetworkObjectField.CreateStringField(name));
            channelJoin.AddField(2, NetworkObjectField.CreateStringField(GetShortcut()));
            member.EnqueueMessage(ObjectOpcode.Channel, channelJoin);
        }

        /// <summary>
        /// 
        /// </summary>
        public override void RemoveMember(Session member)
        {
            base.AddMember(member);
            member.Channels.Remove(this);

            var channelLeave = new NetworkObject();
            channelLeave.AddField(0, NetworkObjectField.CreateIntField((int)ServerChannelAction.Leave));
            channelLeave.AddField(1, NetworkObjectField.CreateStringField(name));
            channelLeave.AddField(2, NetworkObjectField.CreateStringField(GetShortcut()));
            member.EnqueueMessage(ObjectOpcode.Channel, channelLeave);
        }

        /// <summary>
        /// 
        /// </summary>
        public void BroadcastMessage(Session member, string message)
        {
            if (!HasMember(member.Character))
                return;

            var channelMessage = new NetworkObject();
            channelMessage.AddField(0, NetworkObjectField.CreateIntField((int)ServerChannelAction.Message));
            channelMessage.AddField(1, NetworkObjectField.CreateObjectField(member.Character.ToNetworkObject()));
            channelMessage.AddField(2, NetworkObjectField.CreateStringField(name));
            channelMessage.AddField(3, NetworkObjectField.CreateStringField(message));
            channelMessage.AddField(4, NetworkObjectField.CreateStringField(GetShortcut()));
            BroadcastMessage(ObjectOpcode.Channel, channelMessage);
        }
    }
}
