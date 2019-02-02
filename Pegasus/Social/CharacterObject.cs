using System;
using Pegasus.Network.Packet.Object;

namespace Pegasus.Social
{
    public class CharacterObject : Class15, IEquatable<CharacterObject>
    {
        public string Server { get; set; }
        public uint Guid { get; set; }
        public string Name { get; set; }
        public string Unknown1 { get; set; }
        public uint Sequence { get; set; }

        public override NetworkObject ToNetworkObject()
        {
            NetworkObject networkObject = new NetworkObject();
            networkObject.AddField(0, NetworkObjectField.CreateStringField(Server));
            networkObject.AddField(1, NetworkObjectField.CreateStringField(Name));
            networkObject.AddField(2, NetworkObjectField.CreateIntField((int)Guid));
            networkObject.AddField(3, NetworkObjectField.CreateLongField(Sequence));
            networkObject.AddField(4, NetworkObjectField.CreateStringField(Unknown1));
            return networkObject;
        }

        public override void FromNetworkObject(NetworkObject networkObject)
        {
            Server   = NetworkObjectField.ReadStringField(networkObject.GetField(0));
            Name     = NetworkObjectField.ReadStringField(networkObject.GetField(1));
            Guid     = (uint)NetworkObjectField.ReadIntField(networkObject.GetField(2));
            Sequence = (uint)NetworkObjectField.ReadLongField(networkObject.GetField(3));
            Unknown1 = NetworkObjectField.ReadStringField(networkObject.GetField(4));

            Unknown1 = Name; // ??
        }

        public bool Equals(CharacterObject other)
        {
            if (ReferenceEquals(null, other))
                return false;
            if (ReferenceEquals(this, other))
                return true;

            return string.Equals(Server, other.Server) && Guid == other.Guid && string.Equals(Name, other.Name) && string.Equals(Unknown1, other.Unknown1) && Sequence == other.Sequence;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
                return false;
            if (ReferenceEquals(this, obj))
                return true;
            if (obj.GetType() != this.GetType())
                return false;

            return Equals((CharacterObject)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = (Server != null ? Server.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (int)Guid;
                hashCode = (hashCode * 397) ^ (Name != null ? Name.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Unknown1 != null ? Unknown1.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (int)Sequence;
                return hashCode;
            }
        }

        public static bool operator ==(CharacterObject left, CharacterObject right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(CharacterObject left, CharacterObject right)
        {
            return !Equals(left, right);
        }
    }
}
