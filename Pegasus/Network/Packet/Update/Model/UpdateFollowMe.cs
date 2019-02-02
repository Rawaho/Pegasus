using System.IO;

namespace Pegasus.Network.Packet.Update.Model
{
    [UpdateMessage(UpdateType.FollowMe)]
    public class UpdateFollowMe : IReadable, IWritable
    {
        public int Guid { get; set; }
        public string Ftn { get; set; }

        public void Read(BinaryReader reader)
        {
            Guid = reader.ReadInt32();
            Ftn  = reader.ReadPackedString();
        }

        public void Write(BinaryWriter writer)
        {
            writer.Write(Guid);
            writer.WritePackedString(Ftn);
        }
    }
}
