using System.Collections.Generic;
using System.Linq;
using Pegasus.Network;
using Pegasus.Network.Packet.Update;
using Pegasus.Social;

namespace Pegasus.Map
{
    public static class CharacterUpdateManager
    {
        private static readonly Dictionary<CharacterObject, CharacterUpdateInfo> characterInfo = new Dictionary<CharacterObject, CharacterUpdateInfo>();

        public static void SignIn(CharacterObject character)
        {
            characterInfo.Add(character, new CharacterUpdateInfo());
        }

        public static void SignOut(CharacterObject character)
        {
            characterInfo.Remove(character);
        }

        /// <summary>
        /// Broadcast <see cref="ServerUpdatePacket"/>, the way the packet will be broadcast will dictated by the supplied <see cref="UpdateParameters"/>.
        /// </summary>
        public static void BroadcastUpdate(Session session, ServerUpdatePacket packet, UpdateParameters parameters)
        {
            if (!string.IsNullOrEmpty(parameters.Fellowship))
                BroadcastUpdate(session, packet, parameters.Fellowship);
            else if (parameters.Sequence != 0u)
                BroadcastUpdate(session, packet, parameters.Sequence);
            else
                BroadcastUpdate(session, packet);
        }

        /// <summary>
        /// Broadcast <see cref="ServerUpdatePacket"/> to all session fellowships.
        /// </summary>
        public static void BroadcastUpdate(Session session, ServerUpdatePacket packet)
        {
            session.Fellowships.ForEach(f => f.BroadcastMessage(packet));
        }

        /// <summary>
        /// Broadcast <see cref="ServerUpdatePacket"/> to session fellowship.
        /// </summary>
        public static void BroadcastUpdate(Session session, ServerUpdatePacket packet, string fellowship, bool excludeSelf = false)
        {
            session.Fellowships.SingleOrDefault(f => f.Info.Name == fellowship)?.BroadcastMessage(packet, excludeSelf ? session.Character : null);
        }

        /// <summary>
        /// Broadcast <see cref="ServerUpdatePacket"/> to character.
        /// </summary>
        public static void BroadcastUpdate(Session session, ServerUpdatePacket packet, uint sequence)
        {
            session.Fellowships.ForEach(f => f.BroadcastMessage(packet, sequence));
        }

        [UpdatePacketHandler(UpdateType.Initial)]
        public static void UpdateInitial(Session session, ClientUpdateInitial initial, UpdateParameters parameters)
        {
            if (!characterInfo.TryGetValue(session.Character, out CharacterUpdateInfo info))
                return;

            info.Level    = (uint)initial.Class6.int_0;
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

            BroadcastUpdate(session, new ServerUpdateInitial(session.Character.Sequence)
            {
                InitialUpdate = initial.InitialUpdate,
                Class6        = initial.Class6
            });
        }

        [UpdatePacketHandler(UpdateType.Health)]
        public static void UpdateHealth(Session session, ClientUpdateVital vital, UpdateParameters parameters)
        {
            if (!characterInfo.TryGetValue(session.Character, out CharacterUpdateInfo info))
                return;

            info.Health = new CharacterUpdateInfo.Vital
            {
                Current = vital.Current,
                Maximum = vital.Maximum
            };

            BroadcastUpdate(session, new ServerUpdateVital(session.Character.Sequence, UpdateType.Health)
            {
                Current = vital.Current,
                Maximum = vital.Maximum
            });
        }

        [UpdatePacketHandler(UpdateType.Stamina)]
        public static void UpdateStamina(Session session, ClientUpdateVital vital, UpdateParameters parameters)
        {
            if (!characterInfo.TryGetValue(session.Character, out CharacterUpdateInfo info))
                return;

            info.Stamina = new CharacterUpdateInfo.Vital
            {
                Current = vital.Current,
                Maximum = vital.Maximum
            };

            BroadcastUpdate(session, new ServerUpdateVital(session.Character.Sequence, UpdateType.Stamina)
            {
                Current = vital.Current,
                Maximum = vital.Maximum
            });
        }

        [UpdatePacketHandler(UpdateType.Mana)]
        public static void UpdateMana(Session session, ClientUpdateVital vital, UpdateParameters parameters)
        {
            if (!characterInfo.TryGetValue(session.Character, out CharacterUpdateInfo info))
                return;

            info.Mana = new CharacterUpdateInfo.Vital
            {
                Current = vital.Current,
                Maximum = vital.Maximum
            };

            BroadcastUpdate(session, new ServerUpdateVital(session.Character.Sequence, UpdateType.Mana)
            {
                Current = vital.Current,
                Maximum = vital.Maximum
            });
        }

        [UpdatePacketHandler(UpdateType.Location)]
        public static void UpdateLocation(Session session, ClientUpdateLocation location, UpdateParameters parameters)
        {
            if (!characterInfo.TryGetValue(session.Character, out CharacterUpdateInfo info))
                return;

            info.Location = location.Location;
        }

        // ----------------------------------------------------------------------

        [UpdatePacketHandler(UpdateType.SpellCastAttempt)]
        public static void UpdateSpellCastAttempt(Session session, ClientUpdateSpellCastAttempt spellCastAttempt, UpdateParameters parameters)
        {
            BroadcastUpdate(session, new ServerUpdateSpellCastAttempt(session.Character.Sequence)
            {
                Target  = spellCastAttempt.Target,
                SpellId = spellCastAttempt.SpellId,
                Skill   = spellCastAttempt.Skill
            });
        }

        [UpdatePacketHandler(UpdateType.SpellCastComplete)]
        public static void UpdateSpellCastComplete(Session session, ClientUpdateSpellCastComplete spellCastComplete, UpdateParameters parameters)
        {
            BroadcastUpdate(session, new ServerUpdateSpellCastComplete(session.Character.Sequence)
            {
                Target   = spellCastComplete.Target,
                SpellId  = spellCastComplete.SpellId,
                Duration = spellCastComplete.Duration
            });
        }

        [UpdatePacketHandler(UpdateType.AllSpellsExpired)]
        public static void UpdateAllSpellsExpired(Session session, ClientUpdateAllSpellsExpired allSpellsExpired, UpdateParameters parameters)
        {
            BroadcastUpdate(session, new ServerUpdateAllSpellsExpired(session.Character.Sequence)
            {
                SpellId = allSpellsExpired.SpellId,
                Target  = allSpellsExpired.Target
            });
        }

        [UpdatePacketHandler(UpdateType.ProfileChanged)]
        public static void UpdateProfileChanged(Session session, ClientUpdateProfileChanged profileChanged, UpdateParameters parameters)
        {

        }

        [UpdatePacketHandler(UpdateType.FollowMe)]
        public static void UpdateFollowMe(Session session, ClientUpdateFollowMe followMe, UpdateParameters parameters)
        {
            BroadcastUpdate(session, new ServerUpdateFollowMe
            {
                Guid = followMe.Guid,
                Ftn  = followMe.Ftn
            }, parameters);
        }

        [UpdatePacketHandler(UpdateType.Stop)]
        public static void UpdateStop(Session session, ClientUpdateEmpty empty, UpdateParameters parameters)
        {
            BroadcastUpdate(session, new ServerUpdateEmpty(0u, UpdateType.Stop, UpdateFlag.flag_1), parameters);
        }

        [UpdatePacketHandler(UpdateType.ForceBuff)]
        public static void UpdateForceBuff(Session session, ClientUpdateEmpty empty, UpdateParameters parameters)
        {
            BroadcastUpdate(session, new ServerUpdateEmpty(0u, UpdateType.ForceBuff, UpdateFlag.flag_1), parameters.Fellowship, true);
        }

        [UpdatePacketHandler(UpdateType.ForceBuffCancel)]
        public static void UpdateForceBuffCancel(Session session, ClientUpdateEmpty empty, UpdateParameters parameters)
        {
            BroadcastUpdate(session, new ServerUpdateEmpty(0u, UpdateType.ForceBuffCancel, UpdateFlag.flag_1), parameters.Fellowship, true);
        }

        [UpdatePacketHandler(UpdateType.SetSetting)]
        public static void UpdateSetting(Session session, ClientUpdateSetting setting, UpdateParameters parameters)
        {
            BroadcastUpdate(session, new ServerUpdateSetting
            {
                Setting = setting.Setting,
                Value   = setting.Value
            }, parameters);
        }

        [UpdatePacketHandler(UpdateType.Profile)]
        public static void UpdateProfile(Session session, ClientUpdateProfile profile, UpdateParameters parameters)
        {
            BroadcastUpdate(session, new ServerUpdateProfile
            {
                Profile = profile.Profile
            }, parameters.Fellowship, true);
        }

        [UpdatePacketHandler(UpdateType.Use)]
        public static void UpdateUse(Session session, ClientUpdateUse use, UpdateParameters parameters)
        {
            BroadcastUpdate(session, new ServerUpdateUse
            {
                Guid = use.Guid
            }, parameters);
        }

    }
}
