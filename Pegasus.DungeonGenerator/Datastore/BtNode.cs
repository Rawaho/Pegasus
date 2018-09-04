using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;

namespace Pegasus.DungeonGenerator.Datastore
{
    [StructLayout(LayoutKind.Sequential)]
    public class BtNode
    {
        [StructLayout(LayoutKind.Sequential)]
        public struct BtEntry
        {
            public uint Bf;
            public uint Id;
            public uint Offset;
            public uint Size;
            public uint Date;
            public uint Iter;
        }

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x3E)]
        public uint[] NextNode;
        public uint NumEntries;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x3D)]
        public BtEntry[] Entry;

        /// <summary>
        /// Read all <see cref="File"/> entries in a node recursively.
        /// </summary>
        public static void Read(HashSet<File> files, BinaryReader reader, uint blockSize, long sectorOffset)
        {
            uint nodeBlocks = (uint)Marshal.SizeOf<BtNode>() / blockSize + 1u;
            byte[] nodeData = new byte[nodeBlocks * blockSize];

            // read enough sectors for the node
            for (uint i = 0u; i < nodeBlocks; i++)
            {
                Debug.Assert(reader.BaseStream.Length > sectorOffset + blockSize);

                reader.BaseStream.Position = sectorOffset;
                sectorOffset = reader.ReadUInt32();
                reader.Read(nodeData, (int)(i * (blockSize - sizeof(uint))), (int)(blockSize - sizeof(uint)));
            }

            var btNode = nodeData.DeSerialise<BtNode>();

            // file all the files in the current tree node
            for (uint i = 0u; i < btNode.NumEntries; i++)
            {
                var file = new File(reader, blockSize, btNode.Entry[i]);
                Debug.Assert(!files.Contains(file));
                files.Add(file);
            }

            // read any children nodes this parent may have
            for (uint i = 0u; i < (btNode.NextNode[0] == 0u ? 0u : btNode.NumEntries + 1u); i++)
                Read(files, reader, blockSize, btNode.NextNode[i]);
        }
    }
}
