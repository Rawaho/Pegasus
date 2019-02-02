using System;

namespace Pegasus.Network.Packet.Update
{
    [AttributeUsage(AttributeTargets.Class)]
    public class UpdateMessageAttribute : Attribute
    {
        public UpdateType UpdateType { get; }

        public UpdateMessageAttribute(UpdateType updateType)
        {
            UpdateType = updateType;
        }
    }
}
