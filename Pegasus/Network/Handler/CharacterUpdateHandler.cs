using System.IO;
using Pegasus.Network.Packet;
using Pegasus.Network.Packet.Raw;
using Pegasus.Network.Packet.Update;

namespace Pegasus.Network.Handler
{
    public static class CharacterUpdateHandler
    {
        [RawPacketHandler(ClientRawOpcode.CharacterFellowshipUpdate)]
        public static void HandleCharacterFellowshipUpdate(Session session, ClientCharacterFellowshipUpdate packet)
        {
            HandleUpdate(session, packet.UpdateType, packet.Payload, new UpdateParameters(packet.Fellowship));
        }

        [RawPacketHandler(ClientRawOpcode.CharacterSequenceUpdate)]
        public static void HandleCharacterSequenceUpdate(Session session, ClientCharacterSequenceUpdate packet)
        {
            HandleUpdate(session, packet.UpdateType, packet.Payload, new UpdateParameters(sequence: packet.Sequence));
        }

        [RawPacketHandler(ClientRawOpcode.CharacterUpdate)]
        public static void HandleCharacterUpdate(Session session, ClientCharacterUpdate packet)
        {
            HandleUpdate(session, packet.UpdateType, packet.Payload, new UpdateParameters());
        }

        private static void HandleUpdate(Session session, UpdateType updateType, byte[] payload, UpdateParameters parameters)
        {
            ClientUpdatePacket updatePacket = PacketManager.CreateUpdatePacket(updateType);
            if (updatePacket == null)
                return;

            using (var stream = new MemoryStream(payload))
                using (var reader = new BinaryReader(stream))
                    updatePacket.ReadUpdate(reader);

            PacketManager.InvokeUpdatePacketHandler(session, updateType, updatePacket, parameters);
        }
    }
}
