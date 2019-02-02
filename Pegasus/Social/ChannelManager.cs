using System;
using System.Collections.Generic;

namespace Pegasus.Social
{
    public static class ChannelManager
    {
        private static readonly Dictionary<string, Channel> channels = new Dictionary<string, Channel>(StringComparer.InvariantCultureIgnoreCase);

        public static void Update(double lastTick)
        {
            foreach (Channel channel in channels.Values)
                channel.Update(lastTick);
        }

        /// <summary>
        /// Return <see cref="Channel"/> with supplied name, if channel doesn't exist it will be created.
        /// </summary>
        public static Channel GetChannel(string name)
        {
            if (!channels.TryGetValue(name, out Channel channel))
            {
                channel = new Channel(name);
                channels.Add(name, channel);
            }

            return channel;
        }

        public static void BroadcastMessage(string message)
        {

        }
    }
}
