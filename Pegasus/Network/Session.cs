using System.Collections.Generic;
using Pegasus.Database.Model;
using Pegasus.Map;
using Pegasus.Social;

namespace Pegasus.Network
{
    public class Session : NetworkSession
    {
        /// <summary>
        /// Virindi Integrator account information.
        /// </summary>
        public Account Account { get; private set; }

        public string CharacterAccount { get; private set; }
        public CharacterObject Character { get; private set; }

        public CharacterFriendManager FriendManager { get; } = new CharacterFriendManager();

        public List<Channel> Channels { get; } = new List<Channel>();
        public List<Fellowship> Fellowships { get; } = new List<Fellowship>();

        public void SignIn(Account accountInfo, string characterAccount, CharacterObject characterObject)
        {
            Account          = accountInfo;
            CharacterAccount = characterAccount;
            Character        = characterObject;

            State = SessionState.SignedIn;

            FriendManager.Initialise(this);
            CharacterUpdateManager.Register(characterObject);
        }

        public override void Disconnect()
        {
            FriendManager.Disconnect();

            if (Character != null)
                CharacterUpdateManager.Deregister(Character);

            Channels.ForEach(c => c.RemoveMember(Character));
            Channels.Clear();
            Fellowships.ForEach(f => f.RemoveMember(Character));
            Fellowships.Clear();

            base.Disconnect();
            NetworkManager.RemoveSession(this);
        }
    }
}
