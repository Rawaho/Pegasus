using System;
using System.Collections.Generic;
using Pegasus.Network;
using Pegasus.Network.Packet.Object;

namespace Pegasus.Social
{
    public abstract class SocalBase : IUpdate
    {
        protected readonly Dictionary<uint, CharacterObject> members = new Dictionary<uint, CharacterObject>();

        private readonly Queue<CharacterObject> pendingAdd = new Queue<CharacterObject>();
        private readonly Queue<CharacterObject> pendingRemove = new Queue<CharacterObject>();

        public void Update(double lastTick)
        {
            while (pendingAdd.TryDequeue(out CharacterObject character))
                OnAddMember(character);

            while (pendingRemove.TryDequeue(out CharacterObject character))
                OnRemoveMember(character);
        }

        /// <summary>
        /// Returns whether the supplied <see cref="CharacterObject"/> is a member.
        /// </summary>
        public bool HasMember(CharacterObject character)
        {
            return members.ContainsKey(character.Sequence);
        }

        /// <summary>
        /// Enqueue <see cref="CharacterObject"/> to be added as a member.
        /// </summary>
        public void AddMember(CharacterObject character)
        {
            if (HasMember(character))
                return;

            pendingAdd.Enqueue(character);
        }

        /// <summary>
        /// Enqueue <see cref="CharacterObject"/> to be removed as a member.
        /// </summary>
        public void RemoveMember(CharacterObject character)
        {
            if (!HasMember(character))
                return;

            pendingRemove.Enqueue(character);
        }

        protected abstract void OnAddMember(CharacterObject character);
        protected abstract void OnRemoveMember(CharacterObject character);

        /// <summary>
        /// Broadcast <see cref="IWritable"/> to all members.
        /// </summary>
        public void BroadcastMessage(IWritable message)
        {
            foreach (CharacterObject character in members.Values)
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
            foreach (CharacterObject character in members.Values)
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
            foreach (CharacterObject character in members.Values)
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
            foreach (CharacterObject character in members.Values)
            {
                if (!func(character))
                    continue;

                Session session = NetworkManager.FindSessionByCharacter(character);
                session?.EnqueueMessage(opcode, message);
            }
        }
    }
}
