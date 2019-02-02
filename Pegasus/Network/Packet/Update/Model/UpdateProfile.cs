using System.IO;

namespace Pegasus.Network.Packet.Update.Model
{
    [UpdateMessage(UpdateType.Profile)]
    public class UpdateProfile : IReadable, IWritable
    {
        public string Profile { get; set; }

        public void Read(BinaryReader reader)
        {
            Profile = reader.ReadPackedString();
        }

        public void Write(BinaryWriter writer)
        {
            writer.WritePackedString(Profile);
        }
    }
}
