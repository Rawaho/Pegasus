using System.Collections.Generic;
using System.Linq;
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
            CharacterUpdateManager.SignIn(characterObject);
        }

        public override void Disconnect()
        {
            FriendManager.Disconnect();

            if (Character != null)
                CharacterUpdateManager.SignOut(Character);

            Channels.ToList().ForEach(c => c.RemoveMember(this));
            Channels.Clear();
            Fellowships.ToList().ForEach(f => f.RemoveMember(this));
            Fellowships.Clear();

            base.Disconnect();
            NetworkManager.RemoveSession(this);
        }
    }
}
