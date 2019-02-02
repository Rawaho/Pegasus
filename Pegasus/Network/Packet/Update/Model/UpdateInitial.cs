using System.IO;
using Pegasus.Network.Packet.Object;
using Pegasus.Network.Packet.Update.Structure;

namespace Pegasus.Network.Packet.Update.Model
{
    [UpdateMessage(UpdateType.Initial)]
    public class UpdateInitial : IReadable, IWritable
    {
        public InitialUpdateStructure InitialUpdate { get; set; }
        public Class6 Class6 { get; } = new Class6();

        public void Read(BinaryReader reader)
        {
            InitialUpdate = reader.ReadStruct<InitialUpdateStructure>();
            Class6.Read(reader);
        }

        public void Write(BinaryWriter writer)
        {
            writer.WriteStruct(InitialUpdate);
            Class6.Write(writer);
        }
    }
}
