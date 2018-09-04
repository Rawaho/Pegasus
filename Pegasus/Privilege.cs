using System;

namespace Pegasus
{
    [Flags]
    public enum Privilege
    {
        None    = 0x0000,
        flag_1  = 0x0001,
        flag_2  = 0x0002,
        flag_3  = 0x0004,
        flag_4  = 0x0008,
        flag_5  = 0x0010,
        flag_6  = 0x0020,
        flag_7  = 0x0040,
        flag_8  = 0x0080,
        flag_9  = 0x0100,
        flag_10 = 0x0200,
        flag_11 = 0x0400,
        All = flag_1 | flag_2 | flag_3 | flag_4 | flag_5 | flag_6 | flag_7 | flag_8 | flag_9 | flag_10 | flag_11
    }
}
