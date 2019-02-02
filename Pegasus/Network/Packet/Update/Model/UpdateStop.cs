using System.IO;

namespace Pegasus.Network.Packet.Update.Model
{
    [UpdateMessage(UpdateType.Stop)]
    public class UpdateStop : IReadable, IWritable
    {
        public void Read(BinaryReader reader)
        {
        }

        public void Write(BinaryWriter writer)
        {
        }
    }
}
