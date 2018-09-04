
namespace Pegasus.Network.Packet.Update
{
    public struct UpdateParameters
    {
        public string Fellowship { get; }
        public uint Sequence { get; }

        public UpdateParameters(string fellowship = "", uint sequence = 0u)
        {
            Fellowship = fellowship;
            Sequence   = sequence;
        }
    }
}
