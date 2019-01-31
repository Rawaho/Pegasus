using System;
using System.Collections.Generic;

namespace Pegasus.Database.Model
{
    public partial class Account
    {
        public Account()
        {
            FriendFriend1Navigation = new HashSet<Friend>();
        }

        public uint Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public short Privileges { get; set; }
        public string CreateIp { get; set; }
        public DateTime CreateTime { get; set; }
        public string LastIp { get; set; }
        public DateTime LastTime { get; set; }

        public virtual Friend FriendIdNavigation { get; set; }
        public virtual ICollection<Friend> FriendFriend1Navigation { get; set; }
    }
}
