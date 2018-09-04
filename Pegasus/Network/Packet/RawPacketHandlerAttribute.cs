using System;
using Pegasus.Network.Packet.Raw;

namespace Pegasus.Network.Packet
{
    [AttributeUsage(AttributeTargets.Method)]
    public class RawPacketHandlerAttribute : Attribute
    {
        public ClientRawOpcode Opcode { get; }

        public RawPacketHandlerAttribute(ClientRawOpcode opcode)
        {
            Opcode = opcode;
        }
    }
}
