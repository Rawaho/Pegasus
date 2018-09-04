using System;
using Pegasus.Map;

namespace Pegasus.Network.Packet.Update
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class UpdatePacketAttribute : Attribute
    {
        public UpdateType UpdateType { get; }

        public UpdatePacketAttribute(UpdateType updateType)
        {
            UpdateType = updateType;
        }
    }
}
