using System;
using Pegasus.Network.Packet.Object;

namespace Pegasus.Social
{
    public class FellowshipObject : Class15, IEquatable<FellowshipObject>
    {
        public string Name { get; private set; }
        public Enum16 Unknown1 { get; private set; }

        public FellowshipObject()
        {
        }

        public FellowshipObject(string name, Enum16 unknown1)
        {
            Name = name;
            Unknown1 = unknown1;
        }

        public override NetworkObject ToNetworkObject()
        {
            var fellowship = new NetworkObject();
            fellowship.AddField(0, NetworkObjectField.CreateStringField(Name));
            fellowship.AddField(1, NetworkObjectField.CreateIntField((int)Unknown1));
            return fellowship;
        }

        public override void FromNetworkObject(NetworkObject fellowship)
        {
            Name     = NetworkObjectField.ReadStringField(fellowship.GetField(0));
            Unknown1 = (Enum16)NetworkObjectField.ReadIntField(fellowship.GetField(1));
        }

        public bool Equals(FellowshipObject other)
        {
            if (ReferenceEquals(null, other))
                return false;
            if (ReferenceEquals(this, other))
                return true;

            return string.Equals(Name, other.Name) && Unknown1 == other.Unknown1;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
                return false;
            if (ReferenceEquals(this, obj))
                return true;
            if (obj.GetType() != this.GetType())
                return false;

            return Equals((FellowshipObject)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((Name != null ? Name.GetHashCode() : 0) * 397) ^ (int)Unknown1;
            }
        }

        public static bool operator ==(FellowshipObject left, FellowshipObject right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(FellowshipObject left, FellowshipObject right)
        {
            return !Equals(left, right);
        }
    }
}
