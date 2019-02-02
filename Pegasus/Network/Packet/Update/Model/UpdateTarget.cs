using System.IO;

namespace Pegasus.Network.Packet.Update.Model
{
    [UpdateMessage(UpdateType.Target)]
    public class UpdateTarget : IReadable, IWritable
    {
        public uint Target { get; set; }

        public void Read(BinaryReader reader)
        {
            Target = reader.ReadUInt32();
        }

        public void Write(BinaryWriter writer)
        {
            writer.Write(Target);
        }
    }
}
