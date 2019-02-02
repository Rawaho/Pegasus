using System;

namespace Pegasus.Network.Packet.Update
{
    [Flags]
    public enum UpdateFlag
    {
        None   = 0x00,
        flag_0 = 0x01,
        VTank  = 0x02
    }
}
