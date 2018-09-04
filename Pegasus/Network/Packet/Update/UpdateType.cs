namespace Pegasus.Network.Packet.Update
{
    public enum UpdateType
    {
        Initial = 1,
        Location = 2,
        Health  = 3,
        Stamina = 4,
        Mana = 5,
        SpellCastAttempt = 6,
        SpellCastComplete = 7,
        AllSpellsExpired = 8,
        FollowMe = 9,
        const_9 = 10,
        Stop = 11,
        const_11 = 12,
        ForceBuff = 13,
        ForceBuffCancel = 14,
        SetSetting = 15,
        const_15 = 0x10,
        ProfileChanged = 0x11,
        const_17 = 0x12,
        Go = 0x13,
        Profile = 0x14,
        Comps = 0x15,
        const_21 = 0x16,
        Use = 0x17,
        const_23 = 0x17,
    }
}
