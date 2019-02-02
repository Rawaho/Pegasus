using System.IO;

namespace Pegasus.Network.Packet.Update.Model
{
    [UpdateMessage(UpdateType.ForceBuff)]
    public class UpdateForceBuff : IReadable, IWritable
    {
        public void Read(BinaryReader reader)
        {
        }

        public void Write(BinaryWriter writer)
        {
        }
    }
}
