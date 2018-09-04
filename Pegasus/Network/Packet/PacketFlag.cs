using System;

namespace Pegasus.Network.Packet
{
    [Flags]
    public enum PacketFlag
    {
        None      = 0x00,
        Encrypted = 0x01,
        Raw       = 0x02
    }
}
