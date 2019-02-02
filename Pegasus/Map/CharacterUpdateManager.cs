using System.Collections.Generic;
using System.Linq;
using Pegasus.Network;
using Pegasus.Network.Packet.Raw.Model;
using Pegasus.Network.Packet.Update;
using Pegasus.Network.Packet.Update.Model;
using Pegasus.Social;

namespace Pegasus.Map
{
    public static class CharacterUpdateManager
    {
        private static readonly Dictionary<CharacterObject, CharacterUpdateInfo> characterInfo = new Dictionary<CharacterObject, CharacterUpdateInfo>();

        public static void Register(CharacterObject character)
        {
            characterInfo.Add(character, new CharacterUpdateInfo());
        }

        public static void Deregister(CharacterObject character)
        {
            characterInfo.Remove(character);
        }

        /// <summary>
        /// Broadcast <see cref="IWritable"/>, the way the packet will be broadcast will dictated by the supplied <see cref="UpdateParameters"/>.
        /// </summary>
        private static void BroadcastUpdate(Session session, IWritable message, UpdateParameters parameters)
        {
            if (!string.IsNullOrEmpty(parameters.Fellowship))
                session.Fellowships.SingleOrDefault(f => f.Info.Name == parameters.Fellowship)?.BroadcastMessage(message);
            else if (parameters.Sequence != 0u)
                session.Fellowships.ForEach(f => f.BroadcastMessage(message, c => c.Sequence == parameters.Sequence));
            else
                BroadcastUpdate(session, message);
        }

        /// <summary>
        /// Broadcast <see cref="IWritable"/> to all session fellowships.
        /// </summary>
        private static void BroadcastUpdate(Session session, IWritable message)
        {
            session.Fellowships.ForEach(f => f.BroadcastMessage(message));
        }

        private static IWritable BuildUpdate(IWritable update, uint sequence = 0u, UpdateFlag updateFlags = UpdateFlag.None)
        {
            if (!PacketManager.GetUpdateType(update, out UpdateType updateType))
                return null;

            return new ServerUpdatePacket
            {
                Sequence    = sequence,
                UpdateType  = updateType,
                UpdateFlags = updateFlags,
                Update      = update
            };
        }

        [UpdateMessageHandler(UpdateType.Initial)]
        public static void UpdateInitial(Session session, UpdateInitial initial, UpdateParameters parameters)
        {
            if (!characterInfo.TryGetValue(session.Character, out CharacterUpdateInfo info))
                return;

            info.Level    = (uint)initial.Class6.Level;
            info.Location = initial.InitialUpdate.Location;

            info.Health = new CharacterUpdateInfo.Vital
            {
                Current = (uint)initial.InitialUpdate.CurHealth,
                Maximum = (uint)initial.InitialUpdate.MaxHealth
            };

            info.Stamina = new CharacterUpdateInfo.Vital
            {
                Current = (uint)initial.InitialUpdate.CurStam,
                Maximum = (uint)initial.InitialUpdate.MaxStam
            };

            info.Mana = new CharacterUpdateInfo.Vital
            {
                Current = (uint)initial.InitialUpdate.CurMana,
                Maximum = (uint)initial.InitialUpdate.MaxMana
            };

            IWritable update = BuildUpdate(initial, session.Character.Sequence);
            BroadcastUpdate(session, update);
        }

        [UpdateMessageHandler(UpdateType.Location)]
        public static void UpdateLocation(Session session, UpdateLocation location, UpdateParameters parameters)
        {
            if (!characterInfo.TryGetValue(session.Character, out CharacterUpdateInfo info))
                return;

            info.Location = location.Location;

            IWritable update = BuildUpdate(location, session.Character.Sequence);
            BroadcastUpdate(session, update);
        }

        [UpdateMessageHandler(UpdateType.Health)]
        [UpdateMessageHandler(UpdateType.Stamina)]
        [UpdateMessageHandler(UpdateType.Mana)]
        public static void UpdateHealth(Session session, UpdateVital vital, UpdateParameters parameters)
        {
            if (!characterInfo.TryGetValue(session.Character, out CharacterUpdateInfo info))
                return;

            var vitalUpdate = new CharacterUpdateInfo.Vital
            {
                Current = vital.Current,
                Maximum = vital.Maximum
            };

            switch (vital)
            {
                case UpdateHealth _:
                    info.Health = vitalUpdate;
                    break;
                case UpdateStamina _:
                    info.Stamina = vitalUpdate;
                    break;
                case UpdateMana _:
                    info.Mana = vitalUpdate;
                    break;
            }

            IWritable update = BuildUpdate(vital, session.Character.Sequence);
            BroadcastUpdate(session, update);
        }

        [UpdateMessageHandler(UpdateType.Target)]
        public static void UpdateTarget(Session session, UpdateTarget target, UpdateParameters parameters)
        {
            if (!characterInfo.TryGetValue(session.Character, out CharacterUpdateInfo info))
                return;

            info.Target = target.Target;

            IWritable update = BuildUpdate(target, session.Character.Sequence);
            BroadcastUpdate(session, update);
        }

        [UpdateMessageHandler(UpdateType.SpellCastAttempt)]
        [UpdateMessageHandler(UpdateType.SpellCastComplete)]
        [UpdateMessageHandler(UpdateType.AllSpellsExpired)]
        [UpdateMessageHandler(UpdateType.ProfileChanged)]
        [UpdateMessageHandler(UpdateType.Comps)]
        public static void UpdateRelay(Session session, IReadable message, UpdateParameters parameters)
        {
            IWritable update = BuildUpdate(message as IWritable, session.Character.Sequence);
            BroadcastUpdate(session, update);
        }

        [UpdateMessageHandler(UpdateType.Stop)]
        [UpdateMessageHandler(UpdateType.FollowMe)]
        [UpdateMessageHandler(UpdateType.ForceBuff)]
        [UpdateMessageHandler(UpdateType.ForceBuffCancel)]
        [UpdateMessageHandler(UpdateType.SetSetting)]
        [UpdateMessageHandler(UpdateType.Profile)]
        [UpdateMessageHandler(UpdateType.Use)]
        public static void UpdateRelayVTank(Session session, IReadable message, UpdateParameters parameters)
        {
            IWritable update = BuildUpdate(message as IWritable, 0u, UpdateFlag.VTank);
            BroadcastUpdate(session, update, parameters);
        }
    }
}
