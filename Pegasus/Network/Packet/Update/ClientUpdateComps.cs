using System.IO;

namespace Pegasus.Network.Packet.Update
{
    [UpdatePacket(UpdateType.Comps)]
    public class ClientUpdateComps : ClientUpdatePacket
    {
        public override void ReadUpdate(BinaryReader reader)
        {
            int count = reader.ReadInt32();
            for (int i = 0; i < count; i++)
            {
                string ll = reader.ReadPackedString();
                uint bb = reader.ReadPackedUInt32();
            }
        }
    }
}
