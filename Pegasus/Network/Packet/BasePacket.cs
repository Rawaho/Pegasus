namespace Pegasus.Network.Packet
{
    public abstract class BasePacket
    {
        public uint Size { get; protected set; }
        public PacketFlag Flags { get; protected set; }

        public byte[] Data { get; protected set; }
    }
}
