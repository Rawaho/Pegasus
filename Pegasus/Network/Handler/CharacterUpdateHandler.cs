using System.IO;
using Pegasus.Network.Packet.Raw;
using Pegasus.Network.Packet.Raw.Model;
using Pegasus.Network.Packet.Update;

namespace Pegasus.Network.Handler
{
    public static class CharacterUpdateHandler
    {
        [RawMessageHandler(ClientRawOpcode.CharacterFellowshipUpdate)]
        public static void HandleCharacterFellowshipUpdate(Session session, ClientCharacterFellowshipUpdate packet)
        {
            HandleUpdate(session, packet.UpdateType, packet.Payload, new UpdateParameters(packet.Fellowship));
        }

        [RawMessageHandler(ClientRawOpcode.CharacterSequenceUpdate)]
        public static void HandleCharacterSequenceUpdate(Session session, ClientCharacterSequenceUpdate packet)
        {
            HandleUpdate(session, packet.UpdateType, packet.Payload, new UpdateParameters(sequence: packet.Sequence));
        }

        [RawMessageHandler(ClientRawOpcode.CharacterUpdate)]
        public static void HandleCharacterUpdate(Session session, ClientCharacterUpdate packet)
        {
            HandleUpdate(session, packet.UpdateType, packet.Payload, new UpdateParameters());
        }

        private static void HandleUpdate(Session session, UpdateType updateType, byte[] payload, UpdateParameters parameters)
        {
            IReadable updatePacket = PacketManager.CreateUpdateMessage(updateType);
            if (updatePacket == null)
                return;

            using (var stream = new MemoryStream(payload))
                using (var reader = new BinaryReader(stream))
                    updatePacket.Read(reader);

            PacketManager.InvokeUpdatePacketHandler(session, updateType, updatePacket, parameters);
        }
    }
}
