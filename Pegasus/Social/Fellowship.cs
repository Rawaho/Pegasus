using Pegasus.Network;
using Pegasus.Network.Packet.Object;

namespace Pegasus.Social
{
    public class Fellowship : SocalBase
    {
        public FellowshipObject Info { get; }

        public Fellowship(FellowshipObject info)
        {
            Info = info;
        }

        protected override void OnAddMember(CharacterObject character)
        {
            Session session = NetworkManager.FindSessionByCharacter(character);
            if (session == null)
                return;

            NetworkObject fellowshipJoin = new NetworkObject();
            fellowshipJoin.AddField(0, NetworkObjectField.CreateIntField((int)FellowshipAction.Join));
            fellowshipJoin.AddField(1, NetworkObjectField.CreateObjectField(Info.ToNetworkObject()));
            session.EnqueueMessage(ObjectOpcode.Fellowship, fellowshipJoin);

            // send existing members to new member
            foreach (CharacterObject existingCharacter in members.Values)
            {
                NetworkObject fellowshipMemberJoinExisting = new NetworkObject();
                fellowshipMemberJoinExisting.AddField(0, NetworkObjectField.CreateIntField((int)FellowshipAction.MemberJoin));
                fellowshipMemberJoinExisting.AddField(1, NetworkObjectField.CreateObjectField(Info.ToNetworkObject()));
                fellowshipMemberJoinExisting.AddField(2, NetworkObjectField.CreateObjectField(existingCharacter.ToNetworkObject()));
                session.EnqueueMessage(ObjectOpcode.Fellowship, fellowshipMemberJoinExisting);
            }

            members.Add(character.Sequence, character);
            session.Fellowships.Add(this);

            // send new member to existing members
            NetworkObject fellowshipMemberJoin = new NetworkObject();
            fellowshipMemberJoin.AddField(0, NetworkObjectField.CreateIntField((int)FellowshipAction.MemberJoin));
            fellowshipMemberJoin.AddField(1, NetworkObjectField.CreateObjectField(Info.ToNetworkObject()));
            fellowshipMemberJoin.AddField(2, NetworkObjectField.CreateObjectField(character.ToNetworkObject()));
            BroadcastMessage(ObjectOpcode.Fellowship, fellowshipMemberJoin);
        }

        protected override void OnRemoveMember(CharacterObject character)
        {
            NetworkObject fellowshipMemberLeave = new NetworkObject();
            fellowshipMemberLeave.AddField(0, NetworkObjectField.CreateIntField((int)FellowshipAction.MemberLeave));
            fellowshipMemberLeave.AddField(1, NetworkObjectField.CreateObjectField(Info.ToNetworkObject()));
            fellowshipMemberLeave.AddField(2, NetworkObjectField.CreateUShortField((ushort)character.Sequence));
            BroadcastMessage(ObjectOpcode.Fellowship, fellowshipMemberLeave);

            members.Remove(character.Sequence);

            Session session = NetworkManager.FindSessionByCharacter(character);
            if (session != null)
            {
                session.Fellowships.Remove(this);

                NetworkObject fellowshipLeave = new NetworkObject();
                fellowshipLeave.AddField(0, NetworkObjectField.CreateIntField((int)FellowshipAction.Leave));
                fellowshipLeave.AddField(1, NetworkObjectField.CreateObjectField(Info.ToNetworkObject()));
                session.EnqueueMessage(ObjectOpcode.Fellowship, fellowshipLeave);
            }
        }
    }
}
