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

        protected override void OnAddMember(CharacterObject character)
        {
            Session session = NetworkManager.FindSessionByCharacter(character);
            if (session == null)
                return;

            session.Channels.Add(this);

            var channelJoin = new NetworkObject();
            channelJoin.AddField(0, NetworkObjectField.CreateIntField((int)ServerChannelAction.Join));
            channelJoin.AddField(1, NetworkObjectField.CreateStringField(name));
            channelJoin.AddField(2, NetworkObjectField.CreateStringField(GetShortcut()));
            session.EnqueueMessage(ObjectOpcode.Channel, channelJoin);

            members.Add(character.Sequence, character);
        }

        protected override void OnRemoveMember(CharacterObject character)
        {
            Session session = NetworkManager.FindSessionByCharacter(character);
            if (session != null)
            {
                session.Channels.Remove(this);

                var channelLeave = new NetworkObject();
                channelLeave.AddField(0, NetworkObjectField.CreateIntField((int)ServerChannelAction.Leave));
                channelLeave.AddField(1, NetworkObjectField.CreateStringField(name));
                channelLeave.AddField(2, NetworkObjectField.CreateStringField(GetShortcut()));
                session.EnqueueMessage(ObjectOpcode.Channel, channelLeave);
            }

            members.Remove(character.Sequence);
        }

        /// <summary>
        /// Broadcast text message from <see cref="CharacterObject"/> to all members.
        /// </summary>
        public void BroadcastMessage(CharacterObject character, string message)
        {
            if (!HasMember(character))
                return;

            var channelMessage = new NetworkObject();
            channelMessage.AddField(0, NetworkObjectField.CreateIntField((int)ServerChannelAction.Message));
            channelMessage.AddField(1, NetworkObjectField.CreateObjectField(character.ToNetworkObject()));
            channelMessage.AddField(2, NetworkObjectField.CreateStringField(name));
            channelMessage.AddField(3, NetworkObjectField.CreateStringField(message));
            channelMessage.AddField(4, NetworkObjectField.CreateStringField(GetShortcut()));
            BroadcastMessage(ObjectOpcode.Channel, channelMessage);
        }
    }
}
