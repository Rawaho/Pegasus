using System.IO;

namespace Pegasus.Network.Packet.Update.Model
{
    [UpdateMessage(UpdateType.ForceBuffCancel)]
    public class UpdateForceBuffCancel : IReadable, IWritable
    {
        public void Read(BinaryReader reader)
        {
        }

        public void Write(BinaryWriter writer)
        {
        }
    }
}
