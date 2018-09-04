using System;

namespace Pegasus.Network.Packet.Update
{
    [AttributeUsage(AttributeTargets.Method)]
    public class UpdatePacketHandlerAttribute : Attribute
    {
        public UpdateType UpdateType { get; }

        public UpdatePacketHandlerAttribute(UpdateType updateType)
        {
            UpdateType = updateType;
        }
    }
}
