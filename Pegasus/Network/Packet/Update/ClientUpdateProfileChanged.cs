using System.Collections.Generic;
using System.IO;

namespace Pegasus.Network.Packet.Update
{
    [UpdatePacket(UpdateType.ProfileChanged)]
    public class ClientUpdateProfileChanged : ClientUpdatePacket
    {
        public Dictionary<string, bool> Settings { get; } = new Dictionary<string, bool>();

        public override void ReadUpdate(BinaryReader reader)
        {
            uint counter = reader.ReadPackedUInt32();
            for (uint i = 0u; i < counter; i++)
                Settings.Add(reader.ReadPackedString(), reader.ReadBoolean());
        }
    }
}
