namespace Pegasus.Network
{
    public class Class16 : Class15
    {
        public string string_0 = "";

        public override NetworkObject ToNetworkObject()
        {
            NetworkObject class2 = new NetworkObject();
            class2.AddField(0, NetworkObjectField.CreateStringField(this.string_0));
            return class2;
        }

        public override void FromNetworkObject(NetworkObject class63_0)
        {
            this.string_0 = NetworkObjectField.ReadStringField(class63_0.GetField(0));
        }
    }
}
