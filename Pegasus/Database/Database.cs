using System.Collections.Generic;
using System.Data;
using System.Linq;
using MySql.Data.MySqlClient;
using Pegasus.Cryptography;
using Pegasus.Database.Data;

namespace Pegasus.Database
{
    public class Database : DatabaseBase
    {
        public static T Read<T>(DataRow row) where T : IReadable, new()
        {
            T obj = new T();
            obj.Read(row);
            return obj;
        }

        protected override void InitialisePreparedStatements()
        {
            AddPreparedStatement(PreparedStatementId.AccountInsert,
                "INSERT INTO `account` (`username`, `password`, `createIp`, `privileges`) VALUES (?, ?, ?, ?);",
                MySqlDbType.VarChar, MySqlDbType.VarChar, MySqlDbType.VarChar, MySqlDbType.UInt16);

            AddPreparedStatement(PreparedStatementId.AccountSelect,
                "SELECT `id`, `username`, `password`, `privileges` FROM `account` WHERE `username` = ?;",
                MySqlDbType.VarChar);

            AddPreparedStatement(PreparedStatementId.AccountUpdate,
                "UPDATE `account` SET `lastIp` = ? WHERE `id` = ?;",
                MySqlDbType.VarChar, MySqlDbType.UInt32);

            // friend list
            AddPreparedStatement(PreparedStatementId.FriendDelete,
                "DELETE FROM `friend` WHERE `id` = ? AND `friend` = ?;",
                MySqlDbType.UInt32, MySqlDbType.UInt32);

            AddPreparedStatement(PreparedStatementId.FriendInsert,
                "INSERT INTO `friend` (`id`, `friend`) VALUES (?, ?);",
                MySqlDbType.UInt32, MySqlDbType.UInt32);

            AddPreparedStatement(PreparedStatementId.FriendSelect,
                "SELECT `account`.`username` FROM `friend` JOIN `account` ON `friend`.`friend` = `account`.`id` WHERE `friend`.`id` = ?;",
                MySqlDbType.UInt32);

            AddPreparedStatement(PreparedStatementId.FriendReverseSelect,
                "SELECT `account`.`username` FROM `friend` JOIN `account` ON `friend`.`id` = `account`.`id` WHERE `friend`.`friend` = ?;",
                MySqlDbType.UInt32);

            // dungeon
            AddPreparedStatement(PreparedStatementId.DungeonSelect,
                "SELECT `landBlockId`, `name` FROM `dungeon`;");

            AddPreparedStatement(PreparedStatementId.DugeonTileSelect,
                "SELECT `landBlockId`, `tileId`, `x`, `y`, `z` FROM `dungeon_tile`;");
        }

        public AccountInfo CreateAccount(string username, string password, string ip, Privilege privileges)
        {
            string bcryptPassword = BCryptProvider.HashPassword(password);
            (int rowsEffected, long lastInsertedId) = ExecutePreparedStatement(PreparedStatementId.AccountInsert, username, bcryptPassword, ip, (ushort)privileges);

            var account = new AccountInfo();
            account.Read((uint)lastInsertedId, username, bcryptPassword, privileges);
            return account;
        }

        public AccountInfo GetAccount(string username)
        {
            MySqlResult result = SelectPreparedStatement(PreparedStatementId.AccountSelect, username);
            return result.HasRows ? Read<AccountInfo>(result.First) : null;
        }

        public void UpdateAccount(uint id, string ip)
        {
            ExecutePreparedStatement(PreparedStatementId.AccountUpdate, ip, id);
        }

        public void RemoveFriend(uint id, uint friendId)
        {
            ExecutePreparedStatement(PreparedStatementId.FriendDelete, id, friendId);
        }

        public void AddFriend(uint id, uint friendId)
        {
            ExecutePreparedStatement(PreparedStatementId.FriendInsert, id, friendId);
        }

        public IEnumerable<string> GetFriends(uint id)
        {
            MySqlResult result = SelectPreparedStatement(PreparedStatementId.FriendSelect, id);
            return result.Select(r => r.Read<string>("username"));
        }

        public IEnumerable<string> GetReverseFriends(uint id)
        {
            MySqlResult result = SelectPreparedStatement(PreparedStatementId.FriendReverseSelect, id);
            return result.Select(r => r.Read<string>("username"));
        }

        public void LogConversation(string sender, string recipient, string message, string senderIp, string recipientIp)
        {
            ExecutePreparedStatement(PreparedStatementId.LogConversation, sender, recipient, message, senderIp, recipientIp);
        }

        public void LogConversation(string channel, string sender, string message, string senderIp)
        {
            ExecutePreparedStatement(PreparedStatementId.LogChannel, channel, sender, message, senderIp);
        }

        public IEnumerable<DungeonInfo> GetDungeons()
        {
            MySqlResult result = SelectPreparedStatement(PreparedStatementId.DungeonSelect);
            return result.Select(Read<DungeonInfo>);
        }

        public IEnumerable<DungeonTileInfo> GetDungeonTiles()
        {
            MySqlResult result = SelectPreparedStatement(PreparedStatementId.DugeonTileSelect);
            return result.Select(Read<DungeonTileInfo>);
        }
    }
}
