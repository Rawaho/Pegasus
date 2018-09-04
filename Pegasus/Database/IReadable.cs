using System.Data;

namespace Pegasus.Database
{
    public interface IReadable
    {
        void Read(DataRow row);
    }
}
