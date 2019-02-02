using System.IO;

namespace Pegasus.Network.Packet
{
    public class ClientPacket : BasePacket
    {
        public ClientPacket(byte[] data)
        {
            Size = (uint)data.Length;
            if (Size == 0u)
                return;

            using (var stream = new MemoryStream(data))
            using (var reader = new BinaryReader(stream))
            {
                Flags = (PacketFlag)reader.ReadPackedUInt32();
                Data  = reader.ReadBytes((int)stream.Remaining());
            }
        }
    }
}
