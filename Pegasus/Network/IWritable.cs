using System.IO;

namespace Pegasus.Network
{
    public interface IWritable
    {
        void Write(BinaryWriter writer);
    }
}
