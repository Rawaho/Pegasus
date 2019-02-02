using System;
using System.Collections.Generic;
using System.Linq;
using Pegasus.Network;
using Pegasus.Network.Packet.Object;

namespace Pegasus.Social
{
    public abstract class SocalBase
    {
        protected readonly List<CharacterObject> members = new List<CharacterObject>();

        public bool HasMember(CharacterObject character)
        {
            return members.Any(m => m == character);
        }

        public virtual void AddMember(Session member)
        {
            if (HasMember(member.Character))
                return;

            members.Add(member.Character);
        }

        public virtual void RemoveMember(Session member)
        {
            if (!HasMember(member.Character))
                return;

            members.Remove(member.Character);
        }

        /// <summary>
        /// Broadcast <see cref="IWritable"/> to all members.
        /// </summary>
        public void BroadcastMessage(IWritable message)
        {
            foreach (CharacterObject character in members)
            {
                Session session = NetworkManager.FindSessionByCharacter(character);
                session?.EnqueueMessage(message);
            }
        }

        /// <summary>
        /// Broadcast <see cref="IWritable"/> to all members that satisfy supplied predicate.
        /// </summary>
        public void BroadcastMessage(IWritable message, Func<CharacterObject, bool> func)
        {
            foreach (CharacterObject character in members)
            {
                if (!func(character))
                    continue;

                Session session = NetworkManager.FindSessionByCharacter(character);
                session?.EnqueueMessage(message);
            }
        }

        /// <summary>
        /// Broadcast <see cref="NetworkObject"/> to all members.
        /// </summary>
        public void BroadcastMessage(ObjectOpcode opcode, NetworkObject message)
        {
            foreach (CharacterObject character in members)
            {
                Session session = NetworkManager.FindSessionByCharacter(character);
                session?.EnqueueMessage(opcode, message);
            }
        }

        /// <summary>
        /// Broadcast <see cref="NetworkObject"/> to all members that satisfy supplied predicate.
        /// </summary>
        public void BroadcastMessage(ObjectOpcode opcode, NetworkObject message, Func<CharacterObject, bool> func)
        {
            foreach (CharacterObject character in members)
            {
                if (!func(character))
                    continue;

                Session session = NetworkManager.FindSessionByCharacter(character);
                session?.EnqueueMessage(opcode, message);
            }
        }
    }
}
