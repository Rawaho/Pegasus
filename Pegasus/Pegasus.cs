using System;
using System.IO;
using System.Reflection;
using NLog;
using NLog.Config;
using Pegasus.Configuration;
using Pegasus.Map;
using Pegasus.Network;
using Pegasus.Social;

namespace Pegasus
{
    internal class Pegasus
    {
        private const string DeployVersion = "0001";

        #if DEBUG
        public const string Title = "Pegasus: Virindi Integrator Server (Debug)";
        #else
        public const string Title = "Pegasus: Virindi Integrator Server (Release) - " + DeployVersion;
        #endif

        private static readonly Logger log = LogManager.GetCurrentClassLogger();

        public static DateTime StartTime { get; } = DateTime.Now;

        private static void Main()
        {
            string location = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            LogManager.Configuration = new XmlLoggingConfiguration(Path.Combine(location, "nlog.config"));

            Directory.SetCurrentDirectory(location);

            Console.Title = Title;
            log.Info("Initialising...");

            try
            {
                ConfigManager.Initialise($"{Directory.GetCurrentDirectory()}\\Config.json");
                PacketManager.Initialise();
                DungeonTileManager.Initialise();
                NetworkManager.Initialise();

                WorldManager.Initialise(lastTick =>
                {
                    NetworkManager.Update(lastTick);
                    FellowshipManager.Update(lastTick);
                    ChannelManager.Update(lastTick);
                });

                log.Info("Ready!");
            }
            catch (Exception exception)
            {
                log.Error(exception);
            }
        }

        public static void Shutdown()
        {
            NetworkManager.Shutdown = true;
            WorldManager.Shutdown = true;
        }
    }
}
