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

        /// <summary>
        /// 
        /// </summary>
        public override void AddMember(Session member)
        {
            NetworkObject fellowshipJoin = new NetworkObject();
            fellowshipJoin.AddField(0, NetworkObjectField.CreateIntField((int)FellowshipAction.Join));
            fellowshipJoin.AddField(1, NetworkObjectField.CreateObjectField(Info.ToNetworkObject()));
            member.EnqueueMessage(ObjectOpcode.Fellowship, fellowshipJoin);

            // send existing members to new member
            foreach (CharacterObject character in members)
            {
                NetworkObject fellowshipMemberJoin2 = new NetworkObject();
                fellowshipMemberJoin2.AddField(0, NetworkObjectField.CreateIntField((int)FellowshipAction.MemberJoin));
                fellowshipMemberJoin2.AddField(1, NetworkObjectField.CreateObjectField(Info.ToNetworkObject()));
                fellowshipMemberJoin2.AddField(2, NetworkObjectField.CreateObjectField(character.ToNetworkObject()));
                member.EnqueueMessage(ObjectOpcode.Fellowship, fellowshipMemberJoin2);
            }

            base.AddMember(member);
            member.Fellowships.Add(this);

            // send new member to existing members
            NetworkObject fellowshipMemberJoin = new NetworkObject();
            fellowshipMemberJoin.AddField(0, NetworkObjectField.CreateIntField((int)FellowshipAction.MemberJoin));
            fellowshipMemberJoin.AddField(1, NetworkObjectField.CreateObjectField(Info.ToNetworkObject()));
            fellowshipMemberJoin.AddField(2, NetworkObjectField.CreateObjectField(member.Character.ToNetworkObject()));
            BroadcastMessage(ObjectOpcode.Fellowship, fellowshipMemberJoin);
        }

        /// <summary>
        /// 
        /// </summary>
        public override void RemoveMember(Session member)
        {
            NetworkObject fellowshipMemberLeave = new NetworkObject();
            fellowshipMemberLeave.AddField(0, NetworkObjectField.CreateIntField((int)FellowshipAction.MemberLeave));
            fellowshipMemberLeave.AddField(1, NetworkObjectField.CreateObjectField(Info.ToNetworkObject()));
            fellowshipMemberLeave.AddField(2, NetworkObjectField.CreateUShortField((ushort)member.Character.Sequence));
            BroadcastMessage(ObjectOpcode.Fellowship, fellowshipMemberLeave);

            base.RemoveMember(member);
            member.Fellowships.Remove(this);

            NetworkObject fellowshipLeave = new NetworkObject();
            fellowshipLeave.AddField(0, NetworkObjectField.CreateIntField((int)FellowshipAction.Leave));
            fellowshipLeave.AddField(1, NetworkObjectField.CreateObjectField(Info.ToNetworkObject()));
            member.EnqueueMessage(ObjectOpcode.Fellowship, fellowshipLeave);
        }
    }
}
