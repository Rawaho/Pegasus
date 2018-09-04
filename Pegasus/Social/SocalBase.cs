using System.Collections.Generic;
using System.Linq;
using Pegasus.Network;
using Pegasus.Network.Packet;

namespace Pegasus.Social
{
    public abstract class SocalBase
    {
        protected readonly List<CharacterObject> members = new List<CharacterObject>();

        public bool HasMember(CharacterObject character)
        {
            return members.Any(m => m == character);
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual void AddMember(Session member)
        {
            if (HasMember(member.Character))
                return;

            members.Add(member.Character);
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual void RemoveMember(Session member)
        {
            if (!HasMember(member.Character))
                return;

            members.Remove(member.Character);
        }

        /// <summary>
        /// 
        /// </summary>
        public void BroadcastMessage(ServerPacket packet, CharacterObject exclude = null)
        {
            foreach (CharacterObject character in members)
            {
                if (exclude != null && character == exclude)
                    continue;

                Session session = NetworkManager.FindSessionByCharacter(character);
                session.EnqueuePacket(packet);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void BroadcastMessage(ServerPacket packet, uint sequence)
        {
            foreach (CharacterObject character in members)
            {
                if (character.Sequence != sequence)
                    continue;

                Session session = NetworkManager.FindSessionByCharacter(character);
                session.EnqueuePacket(packet);
            }
        }
    }
}
