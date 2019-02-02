using System.Collections.Generic;
using System.IO;

namespace Pegasus.Network.Packet.Update.Model
{
    [UpdateMessage(UpdateType.ProfileChanged)]
    public class UpdateProfileChanged : IReadable, IWritable
    {
        public Dictionary<string, bool> Settings { get; } = new Dictionary<string, bool>();

        public void Read(BinaryReader reader)
        {
            uint counter = reader.ReadPackedUInt32();
            for (uint i = 0u; i < counter; i++)
                Settings.Add(reader.ReadPackedString(), reader.ReadBoolean());
        }

        public void Write(BinaryWriter writer)
        {
            writer.WritePackedUInt32((uint)Settings.Count);
            foreach ((string key, bool value) in Settings)
            {
                writer.WritePackedString(key);
                writer.Write(value);
            }
        }
    }
}
