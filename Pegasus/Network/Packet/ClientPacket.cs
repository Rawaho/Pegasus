using System;

namespace Pegasus.Network.Packet
{
    public class ClientPacket
    {
        public bool IsFragmented => data.Length != offset;
        public int Remaining => data.Length - offset;

        private int offset;
        private readonly byte[] data;

        public ClientPacket(int length)
        {
            data = new byte[length];
        }

        public void AddFragment(byte[] payload, int payloadOffset, int payloadLength)
        {
            if (payloadLength == 0)
                return;

            Buffer.BlockCopy(payload, payloadOffset, data, offset, payloadLength);
            offset += payloadLength;
        }

        public byte[] GetData()
        {
            return data;
        }
    }
}
