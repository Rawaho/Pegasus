using System;
using System.Collections.Generic;

namespace Pegasus.Network.Packet.Object
{
    public class NetworkObject
    {
        private Dictionary<int, NetworkObjectField> dictionary_0 = new Dictionary<int, NetworkObjectField>();

        public NetworkObjectField GetField(int int_0)
        {
            if (this.dictionary_0.ContainsKey(int_0))
            {
                return this.dictionary_0[int_0];
            }
            return new NetworkObjectField();
        }

        public void AddField(int int_0, NetworkObjectField class62_0)
        {
            if (this.dictionary_0.ContainsKey(int_0))
            {
                this.dictionary_0.Remove(int_0);
                this.dictionary_0.Add(int_0, class62_0);
            }
            else
            {
                this.dictionary_0.Add(int_0, class62_0);
            }
        }

        public int method_2()
        {
            return this.dictionary_0.Count;
        }

        public bool method_3(int int_0)
        {
            return this.dictionary_0.ContainsKey(int_0);
        }

        public static NetworkObject UnPack(byte[] byte_0)
        {
            int num4;
            NetworkObject class2 = new NetworkObject();
            for (int i = 0; (i + 7) < byte_0.Length; i = num4 + 1)
            {
                byte[] buffer;
                int num2 = ((int)BitConverter.ToUInt32(byte_0, i)) & 0x7fffffff;
                bool flag = (BitConverter.ToUInt32(byte_0, i) & 0x80000000) > 0;
                int length = Math.Abs(BitConverter.ToInt32(byte_0, i + 4));
                num4 = (i + 7) + length;
                if (num4 > (byte_0.Length - 1))
                {
                    num4 = byte_0.Length - 1;
                }
                if (flag)
                {
                    buffer = new byte[length];
                    Array.Copy(byte_0, i + 8, buffer, 0, length);
                    class2.AddField(num2, new NetworkObjectField(UnPack(buffer)));
                }
                else
                {
                    buffer = new byte[length];
                    Array.Copy(byte_0, i + 8, buffer, 0, length);
                    class2.AddField(num2, new NetworkObjectField(buffer));
                }
            }
            return class2;
        }

        public static byte[] Pack(NetworkObject class63_0)
        {
            int num = 0;
            List<byte[]> list = new List<byte[]>();
            foreach (KeyValuePair<int, NetworkObjectField> pair in class63_0.dictionary_0)
            {
                list.Add(pair.Value.method_1());
                num += list[list.Count - 1].Length + 8;
            }
            byte[] destinationArray = new byte[num];
            int destinationIndex = 0;
            int num3 = 0;
            foreach (KeyValuePair<int, NetworkObjectField> pair in class63_0.dictionary_0)
            {
                uint key = (uint)pair.Key;
                if (pair.Value.IsNetworkObject())
                {
                    key |= 0x80000000;
                }
                Array.Copy(BitConverter.GetBytes(key), 0, destinationArray, destinationIndex, 4);
                Array.Copy(BitConverter.GetBytes(list[num3].Length), 0, destinationArray, destinationIndex + 4, 4);
                Array.Copy(list[num3], 0, destinationArray, destinationIndex + 8, list[num3].Length);
                destinationIndex += 8 + list[num3].Length;
                num3++;
            }
            return destinationArray;
        }
    }
}
