using System.Data;

namespace Pegasus.Database.Data
{
    public class AccountInfo : IReadable
    {
        public uint Id { get; private set; }
        public string Username { get; private set; }
        public string Password { get; private set; }
        public Privilege Privileges { get; private set; }

        public void Read(uint id, string username, string password, Privilege privileges)
        {
            Id         = id;
            Username   = username;
            Password   = password;
            Privileges = privileges;
        }

        public void Read(DataRow row)
        {
            Id         = row.Read<uint>("id");
            Username   = row.Read<string>("username");
            Password   = row.Read<string>("password");
            Privileges = row.Read<Privilege>("privileges");
        }
    }
}
