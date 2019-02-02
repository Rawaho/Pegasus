using System.IO;

namespace Pegasus.Network.Packet.Update.Model
{
    [UpdateMessage(UpdateType.SetSetting)]
    public class UpdateSetting : IReadable, IWritable
    {
        public string Setting { get; set; }
        public bool Value { get; set; }

        public void Read(BinaryReader reader)
        {
            Setting = reader.ReadPackedString();
            Value   = reader.ReadBoolean();
        }

        public void Write(BinaryWriter writer)
        {
            writer.WritePackedString(Setting);
            writer.Write(Value);
        }
    }
}
