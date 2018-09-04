using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

namespace Pegasus.Network.Packet.Update.Structure
{
    [StructLayout(LayoutKind.Sequential)]
    public struct WorldLocationStructure
    {
        public double X;
        public double Y;
        public double Z;
        public bool bool_0;
        public int CellId;
        private static Regex regex_0;

        public NetworkObject method_2()
        {
            NetworkObject class2 = new NetworkObject();
            class2.AddField(0, NetworkObjectField.CreateDoubleField(this.X));
            class2.AddField(1, NetworkObjectField.CreateDoubleField(this.Y));
            class2.AddField(2, NetworkObjectField.CreateDoubleField(this.Z));
            return class2;
        }

        public WorldLocationStructure(NetworkObject class63_0)
        {
            this.X = NetworkObjectField.ReadDoubleField(class63_0.GetField(0));
            this.Y = NetworkObjectField.ReadDoubleField(class63_0.GetField(1));
            this.Z = NetworkObjectField.ReadDoubleField(class63_0.GetField(2));
            this.CellId = smethod_3(this.X, this.Y);
            this.bool_0 = false;
        }

        public void method_3(BinaryReader extendedBinaryReader0)
        {
            this.X = extendedBinaryReader0.ReadDouble();
            this.Y = extendedBinaryReader0.ReadDouble();
            this.Z = extendedBinaryReader0.ReadDouble();
            this.CellId = smethod_3(this.X, this.Y);
            this.bool_0 = false;
        }

        public void method_4(BinaryWriter extendedBinaryWriter0)
        {
            extendedBinaryWriter0.Write(this.X);
            extendedBinaryWriter0.Write(this.Y);
            extendedBinaryWriter0.Write(this.Z);
        }

        public WorldLocationStructure(double double_3, double double_4, double double_5)
        {
            this.X = double_3;
            this.Y = double_4;
            this.Z = double_5;
            this.CellId = smethod_3(this.X, this.Y);
            this.bool_0 = false;
        }

        public WorldLocationStructure(double double_3, double double_4, double double_5, int int_1)
        {
            this.X = double_3;
            this.Y = double_4;
            this.Z = double_5;
            this.CellId = int_1;
            this.bool_0 = false;
        }

        public static WorldLocationStructure smethod_0()
        {
            return new WorldLocationStructure
            {
                X = 0.0,
                Y = 0.0,
                Z = 0.0,
                CellId = 0,
                bool_0 = true
            };
        }

        public static bool smethod_1(WorldLocationStructure worldLocation0, WorldLocationStructure worldLocation1)
        {
            if (worldLocation0.bool_0 && worldLocation1.bool_0)
            {
                return true;
            }
            if (worldLocation0.bool_0 || worldLocation1.bool_0)
            {
                return false;
            }
            return (((worldLocation0.X == worldLocation1.X) && (worldLocation0.Y == worldLocation1.Y)) && (worldLocation0.Z == worldLocation1.Z));
        }

        public static bool smethod_2(WorldLocationStructure worldLocation0, WorldLocationStructure worldLocation1)
        {
            return !smethod_1(worldLocation0, worldLocation1);
        }

        public int method_5()
        {
            return (this.CellId & -65536);
        }

        public static int smethod_3(double double_3, double double_4)
        {
            int num = (int)((double_3 + 101.95) / 0.8);
            int num2 = (int)((double_4 + 101.95) / 0.8);
            if (num < 0)
            {
                num = 0;
            }
            if (num2 < 0)
            {
                num2 = 0;
            }
            if (num > 0xfe)
            {
                num = 0xfe;
            }
            if (num2 > 0xfe)
            {
                num2 = 0xfe;
            }
            return ((((num * 0x100) | num2) << 0x10) & -65536);
        }

        public static bool smethod_4(string string_0, out WorldLocationStructure worldLocation0)
        {
            worldLocation0 = smethod_0();
            Match match = regex_0.Match(string_0);
            if (!match.Success)
            {
                return false;
            }
            double num = double.Parse(match.Groups["ewnum"].Value);
            double num2 = double.Parse(match.Groups["nsnum"].Value);
            if (match.Groups["ewchr"].Value.ToLowerInvariant() == "w")
            {
                num *= -1.0;
            }
            if (match.Groups["nschr"].Value.ToLowerInvariant() == "s")
            {
                num2 *= -1.0;
            }
            worldLocation0 = new WorldLocationStructure(num, num2, 0.0);
            return true;
        }

        public override string ToString()
        {
            string str = (this.Y < 0.0) ? "S" : "N";
            string str2 = (this.X < 0.0) ? "W" : "E";
            return (Math.Round(Math.Abs(this.Y), 1).ToString() + str + ", " + Math.Round(Math.Abs(this.X), 1).ToString() + str2);
        }

        static WorldLocationStructure()
        {
            regex_0 = new Regex(@"(?'nsnum'[0-9]{1,2}(\.[0-9]))(?'nschr'[nNsS])[,\ ]*(?'ewnum'[0-9]{1,2}(\.[0-9]))(?'ewchr'[eEwW])");
        }
    }
}
