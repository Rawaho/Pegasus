using System.Collections.Generic;
using System.IO;
using Pegasus.Network.Packet.Update.Shared;

namespace Pegasus.Network.Packet.Update
{
    public class ServerUpdateComps : ServerUpdatePacket
    {
        public List<Comp> Comps { get; set; } = new List<Comp>();

        public ServerUpdateComps(uint sequence)
            : base(sequence, UpdateType.Comps)
        {
        }

        public override void WriteUpdate(BinaryWriter writer)
        {
            writer.Write(Comps.Count);
            Comps.ForEach(c => c.Write(writer));
        }
    }
}
