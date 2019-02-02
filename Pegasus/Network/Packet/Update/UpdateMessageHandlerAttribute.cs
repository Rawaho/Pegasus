using System;

namespace Pegasus.Network.Packet.Update
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class UpdateMessageHandlerAttribute : Attribute
    {
        public UpdateType UpdateType { get; }

        public UpdateMessageHandlerAttribute(UpdateType updateType)
        {
            UpdateType = updateType;
        }
    }
}
