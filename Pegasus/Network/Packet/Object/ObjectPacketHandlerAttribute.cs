using System;

namespace Pegasus.Network.Packet.Object
{
    [AttributeUsage(AttributeTargets.Method)]
    public class ObjectPacketHandlerAttribute : Attribute
    {
        public ObjectOpcode Opcode { get; }

        public ObjectPacketHandlerAttribute(ObjectOpcode opcode)
        {
            Opcode = opcode;
        }
    }
}
