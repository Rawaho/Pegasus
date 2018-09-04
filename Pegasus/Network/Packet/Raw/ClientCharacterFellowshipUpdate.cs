using System.IO;
using Pegasus.Network.Packet.Update;

namespace Pegasus.Network.Packet.Raw
{
    [ClientRawPacket(ClientRawOpcode.CharacterFellowshipUpdate)]
    public class ClientCharacterFellowshipUpdate : ClientRawPacket
    {
        public UpdateType UpdateType { get; private set; }
        public string Fellowship { get; private set; }
        public byte[] Payload { get; private set; }

        public override void Read(BinaryReader reader)
        {
            reader.ReadPackedUInt32(); // flags
            Fellowship = reader.ReadPackedString();
            UpdateType = (UpdateType)reader.ReadPackedUInt32();
            Payload    = reader.ReadBytes((int)(reader.BaseStream.Length - reader.BaseStream.Position));
        }
    }
}
