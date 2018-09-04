using System;

namespace Pegasus.Network.Packet.Raw
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ClientRawPacketAttribute : Attribute
    {
        public ClientRawOpcode Opcode { get; }

        public ClientRawPacketAttribute(ClientRawOpcode opcode)
        {
            Opcode = opcode;
        }
    }
}
