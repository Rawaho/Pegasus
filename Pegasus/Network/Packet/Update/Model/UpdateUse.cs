using System.IO;

namespace Pegasus.Network.Packet.Update.Model
{
    [UpdateMessage(UpdateType.Use)]
    public class UpdateUse : IReadable, IWritable
    {
        public int Guid { get; private set; }

        public void Read(BinaryReader reader)
        {
            Guid = reader.ReadInt32();
        }

        public void Write(BinaryWriter writer)
        {
            writer.Write(Guid);
        }
    }
}
