using System.IO;

namespace Pegasus.Network.Packet.Update
{
    [UpdatePacket(UpdateType.SetSetting)]
    public class ClientUpdateSetting : ClientUpdatePacket
    {
        public string Setting { get; private set; }
        public bool Value { get; private set; }

        public override void ReadUpdate(BinaryReader reader)
        {
            Setting = reader.ReadPackedString();
            Value   = reader.ReadBoolean();
        }
    }
}
