using System.IO;
using Pegasus.Network.Packet.Object;
using Pegasus.Network.Packet.Raw;

namespace Pegasus.Network.Packet
{
    public class ServerPacket : BasePacket
    {
        public ServerPacket(ServerRawOpcode opcode, IWritable message)
        {
            using (var stream = new MemoryStream())
            using (var writer = new BinaryWriter(stream))
            {
                writer.WritePackedUInt32((uint)opcode);
                message.Write(writer);
                Data = stream.ToArray();
            }

            Size  = 1 + (uint)Data.Length;
            Flags = PacketFlag.Raw;
        }

        public ServerPacket(NetworkObject message)
        {
            Data  = NetworkObject.Pack(message);
            Size  = 1 + (uint)Data.Length;
        }
    }
}
