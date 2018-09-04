namespace Pegasus.DungeonGenerator.Datastore
{
    public enum DbType
    {
        None                  = 0x00000000,
        LandBlock             = 0x00000001,
        Lbi                   = 0x00000002,
        Cell                  = 0x00000003,
        Lbo                   = 0x00000004,
        Instantiation         = 0x00000005,

        [DbType(0x01000000, 0x0100FFFF)]
        Gfxobj                = 0x00000006,

        [DbType(0x02000000, 0x0200FFFF)]
        Setup                 = 0x00000007,

        [DbType(0x03000000, 0x0300FFFF)]
        Anim                  = 0x00000008,

        AnimationHook         = 0x00000009,

        [DbType(0x04000000, 0x0400FFFF)]
        Palette               = 0x0000000A,

        [DbType(0x05000000, 0x05FFFFFF)]
        SurfaceTexture        = 0x0000000B,

        [DbType(0x06000000, 0x07FFFFFF)]
        RenderSurface         = 0x0000000C,

        [DbType(0x08000000, 0x0800FFFF)]
        Surface               = 0x0000000D,

        [DbType(0x09000000, 0x0900FFFF)]
        MTable                = 0x0000000E,

        [DbType(0x0A000000, 0x0A00FFFF)]
        Wave                  = 0x0000000F,

        [DbType(0x0D000000, 0x0D00FFFF)]
        Environment           = 0x00000010,

        [DbType(0x0E000007, 0x0E000007)]
        ChatPoseTable         = 0x00000011,

        [DbType(0x0E00000D, 0x0E00000D)]
        ObjectHierarchy       = 0x00000012,

        [DbType(0x0E00001A, 0x0E00001A)]
        BadData               = 0x00000013,

        [DbType(0x0E00001E, 0x0E00001E)]
        TabooTable            = 0x00000014,

        File2IdTable          = 0x00000015,

        [DbType(0x0E000020, 0x0E000020)]
        NameFilterTable       = 0x00000016,

        [DbType(0x0E020000, 0x0E02FFFF)]
        MonitoredProperties   = 0x00000017,

        [DbType(0x0F000000, 0x0F00FFFF)]
        PalSet                = 0x00000018,

        [DbType(0x10000000, 0x1000FFFF)]
        Clothing              = 0x00000019,

        [DbType(0x11000000, 0x1100FFFF)]
        DegradeInfo           = 0x0000001A,

        [DbType(0x12000000, 0x1200FFFF)]
        Scene                 = 0x0000001B,

        [DbType(0x13000000, 0x1300FFFF)]
        Region                = 0x0000001C,

        [DbType(0x14000000, 0x1400FFFF)]
        Keymap                = 0x0000001D,

        [DbType(0x15000000, 0x15FFFFFF)]
        RenderTexture         = 0x0000001E,

        [DbType(0x16000000, 0x16FFFFFF)]
        RenderMaterial        = 0x0000001F,

        [DbType(0x17000000, 0x17FFFFFF)]
        MaterialModifier      = 0x00000020,

        [DbType(0x18000000, 0x18FFFFFF)]
        MaterialInstance      = 0x00000021,

        [DbType(0x20000000, 0x2000FFFF)]
        STable                = 0x00000022,

        [DbType(0x21000000, 0x21FFFFFF)]
        UiLayout              = 0x00000023,

        [DbType(0x22000000, 0x22FFFFFF)]
        EnumMapper            = 0x00000024,

        [DbType(0x23000000, 0x24FFFFFF)]
        StringTable           = 0x00000025,

        [DbType(0x25000000, 0x25FFFFFF)]
        DidMapper             = 0x00000026,

        [DbType(0x26000000, 0x2600FFFF)]
        ActionMap             = 0x00000027,

        [DbType(0x27000000, 0x27FFFFFF)]
        DualDidMapper         = 0x00000028,

        [DbType(0x31000000, 0x3100FFFF)]
        String                = 0x00000029,

        [DbType(0x32000000, 0x3200FFFF)]
        ParticleEmitter       = 0x0000002A,

        [DbType(0x33000000, 0x3300FFFF)]
        PhysicsScript         = 0x0000002B,

        [DbType(0x34000000, 0x3400FFFF)]
        PhysicsScriptTable    = 0x0000002C,

        [DbType(0x39FFFFFF, 0x39FFFFFF)]
        MasterProperty        = 0x0000002D,

        [DbType(0x40000000, 0x40000FFF)]
        Font                  = 0x0000002E,

        [DbType(0x40001000, 0x40FFFFFF)]
        FontLocal             = 0x0000002F,

        [DbType(0x41000000, 0x41FFFFFF)]
        StringState           = 0x00000030,

        [DbType(0x78000000, 0x7FFFFFFF)]
        DbProperties          = 0x00000031,

        [DbType(0x19000000, 0x19FFFFFF)]
        RenderMesh            = 0x00000043,

        [DbType(0x00000001, 0x0000FFFF)]
        WeenieDef             = 0x10000001,

        [DbType(0x0E000002, 0x0E000002)]
        CharGen0              = 0x10000002,

        [DbType(0x0E000003, 0x0E000003)]
        Attribute2ndTable0    = 0x10000003,

        [DbType(0x0E000004, 0x0E000004)]
        SkillTable0           = 0x10000004,

        [DbType(0x0E00000E, 0x0E00000E)]
        SpellTable0           = 0x10000005,

        [DbType(0x0E00000F, 0x0E00000F)]
        SpellComponentTable0  = 0x10000006,

        [DbType(0x0E000011, 0x0E000011)]
        WTreasureSystem       = 0x10000007,

        [DbType(0x0E000019, 0x0E000019)]
        WCraftTable           = 0x10000008,

        [DbType(0x0E000018, 0x0E000018)]
        XpTable0              = 0x10000009,

        [DbType(0x0E00001B, 0x0E00001B)]
        QuestDefDb0           = 0x1000000A,

        [DbType(0x0E00001C, 0x0E00001C)]
        GameEventDb           = 0x1000000B,

        [DbType(0x0E010000, 0x0E01FFFF)]
        QualityFilter0        = 0x1000000C,

        [DbType(0x30000000, 0x3000FFFF)]
        CombatTable0          = 0x1000000D,

        [DbType(0x38000000, 0x3800FFFF)]
        MutateFilter          = 0x1000000E,

        [DbType(0x0E00001D, 0x0E00001D)]
        ContractTable0        = 0x10000010
    }
}
