namespace Pegasus.Configuration
{
    public struct Config
    {
        public struct ConfigNetwork
        {
            public string Host { get; set; }
            public uint Port { get; set; }
        }

        public struct ConfigMySql
        {
            public string Host { get; set; }
            public uint Port { get; set; }
            public string Database { get; set; }
            public string Username { get; set; }
            public string Password { get; set; }
        }

        public ConfigNetwork Network { get; set; }
        public ConfigMySql MySql { get; set; }
    }
}
