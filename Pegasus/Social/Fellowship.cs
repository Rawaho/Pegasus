using System.Collections.Generic;
using System.Linq;
using Pegasus.Network;
using Pegasus.Network.Packet;
using Pegasus.Network.Packet.Update;
using Pegasus.Network.Packet.Update.Structure;

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
            member.EnqueuePacket(new ServerObjectPacket(ObjectOpcode.Fellowship, fellowshipJoin, false));

            // send existing members to new member
            foreach (CharacterObject character in members)
            {
                NetworkObject fellowshipMemberJoin2 = new NetworkObject();
                fellowshipMemberJoin2.AddField(0, NetworkObjectField.CreateIntField((int)FellowshipAction.MemberJoin));
                fellowshipMemberJoin2.AddField(1, NetworkObjectField.CreateObjectField(Info.ToNetworkObject()));
                fellowshipMemberJoin2.AddField(2, NetworkObjectField.CreateObjectField(character.ToNetworkObject()));
                member.EnqueuePacket(new ServerObjectPacket(ObjectOpcode.Fellowship, fellowshipMemberJoin2, false));
            }

            base.AddMember(member);
            member.Fellowships.Add(this);

            // send new member to existing members
            NetworkObject fellowshipMemberJoin = new NetworkObject();
            fellowshipMemberJoin.AddField(0, NetworkObjectField.CreateIntField((int)FellowshipAction.MemberJoin));
            fellowshipMemberJoin.AddField(1, NetworkObjectField.CreateObjectField(Info.ToNetworkObject()));
            fellowshipMemberJoin.AddField(2, NetworkObjectField.CreateObjectField(member.Character.ToNetworkObject()));
            BroadcastMessage(new ServerObjectPacket(ObjectOpcode.Fellowship, fellowshipMemberJoin, false));
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
            BroadcastMessage(new ServerObjectPacket(ObjectOpcode.Fellowship, fellowshipMemberLeave, false));

            base.RemoveMember(member);
            member.Fellowships.Remove(this);

            NetworkObject fellowshipLeave = new NetworkObject();
            fellowshipLeave.AddField(0, NetworkObjectField.CreateIntField((int)FellowshipAction.Leave));
            fellowshipLeave.AddField(1, NetworkObjectField.CreateObjectField(Info.ToNetworkObject()));
            member.EnqueuePacket(new ServerObjectPacket(ObjectOpcode.Fellowship, fellowshipLeave, false));
        }

        public void BroadcastMessage(Session member, string message)
        {

        }
    }
}
