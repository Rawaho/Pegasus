using System;
using System.IO;
using Newtonsoft.Json;
using NLog;

namespace Pegasus.Configuration
{
    public static class ConfigManager
    {
        private static readonly Logger log = LogManager.GetCurrentClassLogger();

        public static Config Config { get; private set; }

        public static void Initialise(string path)
        {
            try
            {
                Config = JsonConvert.DeserializeObject<Config>(File.ReadAllText(path));
            }
            catch (Exception exception)
            {
                log.Fatal(exception);
                throw;
            }
        }
    }
}
