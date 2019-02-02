using System.Collections.Generic;

namespace Pegasus.Social
{
    public static class FellowshipManager
    {
        private static readonly Dictionary<FellowshipObject, Fellowship> fellowships = new Dictionary<FellowshipObject, Fellowship>();

        public static void Update(double lastTick)
        {
            foreach (Fellowship fellowship in fellowships.Values)
                fellowship.Update(lastTick);
        }

        /// <summary>
        /// Return <see cref="Fellowship"/> with supplied name, if fellowship doesn't exist it will be created.
        /// </summary>
        public static Fellowship GetFellowship(FellowshipObject fellowshipInfo)
        {
            if (!fellowships.TryGetValue(fellowshipInfo, out Fellowship fellowship))
            {
                fellowship = new Fellowship(fellowshipInfo);
                fellowships.Add(fellowshipInfo, fellowship);
            }

            return fellowship;
        }
    }
}
