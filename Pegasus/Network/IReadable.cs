using System.IO;

namespace Pegasus.Network
{
    public interface IReadable
    {
        void Read(BinaryReader reader);
    }
}
