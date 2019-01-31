using System.Collections.Generic;
using System.IO;
using Pegasus.Network.Packet.Update.Shared;

namespace Pegasus.Network.Packet.Update
{
    [UpdatePacket(UpdateType.Comps)]
    public class ClientUpdateComps : ClientUpdatePacket
    {
        public List<Comp> Comps { get; } = new List<Comp>();

        public override void ReadUpdate(BinaryReader reader)
        {
            uint count = reader.ReadUInt32();
            for (uint i = 0u; i < count; i++)
            {
                var comp = new Comp();
                comp.Read(reader);
                Comps.Add(comp);
            }
        }
    }
}
