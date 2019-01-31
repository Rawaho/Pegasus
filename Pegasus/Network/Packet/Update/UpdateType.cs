namespace Pegasus.Network.Packet.Update
{
    public enum UpdateType
    {
        Initial           = 1,
        Location          = 2,
        Health            = 3,
        Stamina           = 4,
        Mana              = 5,
        SpellCastAttempt  = 6,
        SpellCastComplete = 7,
        AllSpellsExpired  = 8,
        FollowMe          = 9,
        const_9           = 10,
        Stop              = 11,
        const_11          = 12,
        ForceBuff         = 13,
        ForceBuffCancel   = 14,
        SetSetting        = 15,
        const_15          = 16,
        ProfileChanged    = 17,
        Target            = 18,
        Go                = 19,
        Profile           = 20,
        Comps             = 21,
        const_21          = 22,
        Use               = 23
    }
}
