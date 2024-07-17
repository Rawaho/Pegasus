using System.Collections.Generic;
using System.Linq;
using System.Net;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Pegasus.Cryptography;
using Pegasus.Database.Model;

namespace Pegasus.Database
{
    public static class DatabaseManager
    {
        public static Account CreateAccount(string username, string password, IPAddress ip, Privilege privileges)
        {
            using (var context = new DatabaseContext())
            {
                context.Account.Add(new Account
                {
                    Username   = username,
                    Password   = BCryptProvider.HashPassword(password),
                    CreateIp   = ip.ToString(),
                    Privileges = (short)privileges
                });

                context.SaveChanges();
                return context.Account.SingleOrDefault(a => a.Username == username);
            }
        }

        public static Account GetAccount(string username)
        {
            using (var context = new DatabaseContext())
                return context.Account.SingleOrDefault(a => a.Username == username);
        }

        public static void UpdateAccount(uint id, IPAddress ip)
        {
            using (var context = new DatabaseContext())
            {
                var account = new Account
                {
                    Id     = id,
                    LastIp = ip.ToString()
                };

                EntityEntry<Account> entity = context.Attach(account);
                entity.Property(p => p.LastIp).IsModified = true;

                context.SaveChanges();
            }
        }

        public static void AddFriend(uint id, uint friendId)
        {
            using (var context = new DatabaseContext())
            {
                context.Friend.Add(new Friend
                {
                    Id      = id,
                    Friend1 = friendId
                });

                context.SaveChanges();
            }
        }

        public static void RemoveFriend(uint id, uint friendId)
        {
            using (var context = new DatabaseContext())
            {
                context.Friend.Remove(new Friend
                {
                    Id      = id,
                    Friend1 = friendId
                });

                context.SaveChanges();
            }
        }

        public static List<string> GetFriends(uint id)
        {
            using (var context = new DatabaseContext())
            {
                return context.Friend
                    .Where(f => f.Id == id)
                    .Include(f => f.Friend1Navigation)
                    .Select(f => f.Friend1Navigation.Username)
                    .ToList();
            }
        }

        public static List<string> GetReverseFriends(uint id)
        {
            using (var context = new DatabaseContext())
            {
                return context.Friend
                    .Where(f => f.Friend1 == id)
                    .Include(f => f.IdNavigation)
                    .Select(f => f.IdNavigation.Username)
                    .ToList();
            }
        }

        public static List<Dungeon> GetDungeons()
        {
            using (var context = new DatabaseContext())
                return context.Dungeon
                    .Include(d => d.DungeonTile)
                    .AsNoTracking()
                    .ToList();
        }
    }
}
