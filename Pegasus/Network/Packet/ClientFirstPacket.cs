using System.IO;

namespace Pegasus.Network.Packet
{
    public class ClientFirstPacket : BasePacket
    {
        public ClientFirstPacket(byte[] data)
        {
            Size = (uint)data.Length;

            using (var stream = new MemoryStream(data))
            using (var reader = new BinaryReader(stream))
            {
                Data = reader.ReadBytes((int)stream.Remaining());
            }
        }
    }
}
