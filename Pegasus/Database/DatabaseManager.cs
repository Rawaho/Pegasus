using Pegasus.Configuration;

namespace Pegasus.Database
{
    public static class DatabaseManager
    {
        public static Database Database { get; } = new Database();

        public static void Initialise(Config.ConfigMySql configMySql)
        {
            Database.Initialise(configMySql.Host, configMySql.Port, configMySql.Username,
                configMySql.Password, configMySql.Database);
        }
    }
}
