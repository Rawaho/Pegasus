using System;
using System.IO;

namespace Pegasus.DungeonGenerator.Datastore
{
    public class File
    {
        public uint Id => Entry.Id & 0xFFFFFF;
        public BtNode.BtEntry Entry { get; }
        public byte[] Data { get; }

        public File(BinaryReader reader, uint blockSize, BtNode.BtEntry entry)
        {
            Entry = entry;
            Data  = new byte[entry.Size];

            uint fileBlocks = entry.Size / blockSize + 1u;
            byte[] fileData = new byte[fileBlocks * blockSize];

            // read enough sectors for the file
            for (uint i = 0u; i < fileBlocks; i++)
            {
                reader.BaseStream.Position = entry.Offset;
                entry.Offset = reader.ReadUInt32();
                reader.Read(fileData, (int)(i * (blockSize - sizeof(uint))), (int)(blockSize - sizeof(uint)));
            }

            Buffer.BlockCopy(fileData, 0, Data, 0, (int)entry.Size);
        }
    }
}
