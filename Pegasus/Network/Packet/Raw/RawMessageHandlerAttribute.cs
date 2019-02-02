using System;

namespace Pegasus.Network.Packet.Raw
{
    [AttributeUsage(AttributeTargets.Method)]
    public class RawMessageHandlerAttribute : Attribute
    {
        public ClientRawOpcode Opcode { get; }

        public RawMessageHandlerAttribute(ClientRawOpcode opcode)
        {
            Opcode = opcode;
        }
    }
}
