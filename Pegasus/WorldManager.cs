using System;
using System.Diagnostics;
using System.Threading;
using NLog;
using Pegasus.Network;

namespace Pegasus
{
    public static class WorldManager
    {
        private static readonly Logger log = LogManager.GetCurrentClassLogger();

        public static volatile bool Shutdown = false;

        public static void Initialise()
        {
            StartBackgroundThread();
        }

        private static void StartBackgroundThread()
        {
            new Thread(() =>
            {
                try
                {
                    double lastTick = 0d;
                    var worldTick = new Stopwatch();

                    while (!Shutdown)
                    {
                        worldTick.Restart();

                        NetworkManager.Update(lastTick);

                        Thread.Sleep(1);
                        lastTick = (double)worldTick.ElapsedTicks / Stopwatch.Frequency;
                    }
                }
                catch (Exception exception)
                {
                    log.Error(exception);
                }
            }).Start();
        }
    }
}
