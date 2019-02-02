using System;

namespace Pegasus.Network.Packet.Raw
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ServerRawMessageAttribute : Attribute
    {
        public ServerRawOpcode Opcode { get; }

        public ServerRawMessageAttribute(ServerRawOpcode opcode)
        {
            Opcode = opcode;
        }
    }
}
