using Pegasus.Network.Packet.Update.Structure;

namespace Pegasus.Map
{
    public class CharacterUpdateInfo
    {
        public struct Vital
        {
            public uint Current { get; set; }
            public uint Maximum { get; set; }
        }

        public uint Level { get; set; }
        public WorldLocationStructure Location { get; set; }
        public Vital Health { get; set; }
        public Vital Stamina { get; set; }
        public Vital Mana { get; set; }
    }
}
