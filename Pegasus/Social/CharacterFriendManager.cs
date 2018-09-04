using System;
using System.Collections.Generic;
using System.Linq;
using Pegasus.Database;
using Pegasus.Database.Data;
using Pegasus.Network;
using Pegasus.Network.Packet;

namespace Pegasus.Social
{
    public class CharacterFriendManager
    {
        private Session owner;
        private readonly Dictionary<string, List<CharacterObject>> friendList = new Dictionary<string, List<CharacterObject>>(StringComparer.InvariantCultureIgnoreCase);

        public void Initialise(Session me)
        {
            owner = me;

            foreach (string account in DatabaseManager.Database.GetFriends(owner.Account.Id))
                AddAccount(account);

            foreach (string account in DatabaseManager.Database.GetReverseFriends(owner.Account.Id))
                foreach (Session session in NetworkManager.FindSession(account))
                    session.FriendManager.CharacterSignIn(owner.Account.Username, owner.Character);
        }

        public void Disconnect()
        {
            if (owner == null)
                return;

            foreach (string account in DatabaseManager.Database.GetReverseFriends(owner.Account.Id))
                foreach (Session session in NetworkManager.FindSession(account))
                    session.FriendManager.CharacterSignOut(owner.Account.Username, owner.Character);
        }

        /// <summary>
        /// 
        /// </summary>
        public void AddAccountAll(string account)
        {
            AccountInfo accountInfo = DatabaseManager.Database.GetAccount(account);
            if (accountInfo == null)
                return;

            DatabaseManager.Database.AddFriend(owner.Account.Id, accountInfo.Id);

            IEnumerable<Session> sessions = NetworkManager.FindSession(owner.Account.Username);
            foreach (Session session in sessions)
                session.FriendManager.AddAccount(account);
        }

        /// <summary>
        /// 
        /// </summary>
        private void AddAccount(string account)
        {
            if (friendList.ContainsKey(account))
                return;

            friendList.Add(account, new List<CharacterObject>());            

            IEnumerable<Session> sessions = NetworkManager.FindSession(account);
            foreach (Session session in sessions)
            {
                friendList[account].Add(session.Character);

                NetworkObject characterAdd = new NetworkObject();
                characterAdd.AddField(0, NetworkObjectField.CreateIntField((int)ServerFriendAction.Add));
                characterAdd.AddField(1, NetworkObjectField.CreateStringField(account));
                characterAdd.AddField(2, NetworkObjectField.CreateBoolField(true));
                characterAdd.AddField(3, NetworkObjectField.CreateObjectField(session.Character.ToNetworkObject()));
                owner.EnqueuePacket(new ServerObjectPacket(ObjectOpcode.FriendList, characterAdd, false));
            }

            if (friendList[account].Count == 0)
            {
                NetworkObject accountAdd = new NetworkObject();
                accountAdd.AddField(0, NetworkObjectField.CreateIntField((int)ServerFriendAction.Add));
                accountAdd.AddField(1, NetworkObjectField.CreateStringField(account));
                accountAdd.AddField(2, NetworkObjectField.CreateBoolField(false));
                owner.EnqueuePacket(new ServerObjectPacket(ObjectOpcode.FriendList, accountAdd, false));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void CharacterSignIn(string account, CharacterObject character)
        {
            if (!friendList.ContainsKey(account))
                return;

            friendList[account].Add(character);

            NetworkObject characterSignIn = new NetworkObject();
            characterSignIn.AddField(0, NetworkObjectField.CreateIntField((int)ServerFriendAction.SignIn));
            characterSignIn.AddField(1, NetworkObjectField.CreateStringField(account));
            characterSignIn.AddField(2, NetworkObjectField.CreateObjectField(character.ToNetworkObject()));
            owner.EnqueuePacket(new ServerObjectPacket(ObjectOpcode.FriendList, characterSignIn, false));
        }

        /// <summary>
        /// 
        /// </summary>
        public void RemoveAccountAll(string account)
        {
            AccountInfo accountInfo = DatabaseManager.Database.GetAccount(account);
            if (accountInfo == null)
                return;

            DatabaseManager.Database.RemoveFriend(owner.Account.Id, accountInfo.Id);

            IEnumerable<Session> sessions = NetworkManager.FindSession(owner.Account.Username);
            foreach (Session session in sessions)
                session.FriendManager.RemoveAccount(account);
        }

        /// <summary>
        /// 
        /// </summary>
        public void RemoveAccount(string account)
        {
            if (!friendList.ContainsKey(account))
                return;

            friendList.Remove(account);

            NetworkObject networkObject = new NetworkObject();
            networkObject.AddField(0, NetworkObjectField.CreateIntField((int)ServerFriendAction.Remove));
            networkObject.AddField(1, NetworkObjectField.CreateStringField(account));
            owner.EnqueuePacket(new ServerObjectPacket(ObjectOpcode.FriendList, networkObject, false));
        }

        /// <summary>
        /// 
        /// </summary>
        public void CharacterSignOut(string account, CharacterObject character)
        {
            if (!friendList.ContainsKey(account))
                return;

            friendList[account].Remove(character);

            NetworkObject characterSignOut = new NetworkObject();
            characterSignOut.AddField(0, NetworkObjectField.CreateIntField((int)ServerFriendAction.SignOut));
            characterSignOut.AddField(1, NetworkObjectField.CreateStringField(account));
            characterSignOut.AddField(2, NetworkObjectField.CreateObjectField(character.ToNetworkObject()));
            owner.EnqueuePacket(new ServerObjectPacket(ObjectOpcode.FriendList, characterSignOut, false));
        }
    }
}
