using System.IO;

namespace Pegasus.Network.Packet.Update
{
    public class ServerUpdateSetting : ServerUpdatePacket
    {
        public string Setting { get; set; }
        public bool Value { get; set; }

        public ServerUpdateSetting()
            : base(0u, UpdateType.SetSetting, UpdateFlag.flag_1)
        {
        }

        public override void WriteUpdate(BinaryWriter writer)
        {
            writer.WritePackedString(Setting);
            writer.Write(Value);
        }
    }
}
