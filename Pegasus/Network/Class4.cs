using System.IO;

namespace Pegasus.Network
{
    public abstract class Class4 : Class3
    {
        protected Class4()
        {
        }

        public override NetworkObject ToNetworkObject()
        {
            using (var stream = new MemoryStream())
            {
                using (var writer = new BinaryWriter(stream))
                {
                    this.Write(writer);

                    NetworkObject class2 = new NetworkObject();
                    class2.AddField(0, NetworkObjectField.CreateByteArrayField(stream.ToArray()));
                    return class2;
                }
            }
        }

        public override void FromNetworkObject(NetworkObject class63_0)
        {
            using (var stream = new MemoryStream(NetworkObjectField.smethod_1(class63_0.GetField(0))))
            {
                using (var reader = new BinaryReader(stream))
                {
                    this.Read(reader);
                }
            }
        }
    }
}
