using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using NLog;

namespace Pegasus.DungeonGenerator.Datastore
{
    public abstract class Datastore
    {
        private static readonly Logger log = LogManager.GetCurrentClassLogger();

        [StructLayout(LayoutKind.Sequential)]
        public struct DiskHeaderBlock
        {
            [StructLayout(LayoutKind.Sequential)]
            public struct DiskFileInfo
            {
                public uint Magic;
                public uint BlockSize;
                public uint FileSize;
                public uint DataSet;
                public uint DataSubSet;
                public uint FirstFree;
                public uint FinalFree;
                public uint FreeBlocks;
                public uint BTreeRoot;
                public uint YoungLru;
                public uint OldLru;
                public uint UseLru;
                public uint MasterMapId;
                public uint EnginePackVNum;
                public uint GamePackVNum;
                [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
                public byte[] MajorVNum;
                public uint MinVNum;
            }

            /*[StructLayout(LayoutKind.Sequential)]
            struct DiskTransactionInfo
            {
            }*/

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x100)]
            public byte[] VersionString;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x40)]
            public byte[] TransactionRecord;
            public DiskFileInfo FileInfo;
        }

        protected readonly Dictionary<uint, IUnpackable> objectCache = new Dictionary<uint, IUnpackable>();

        /// <summary>
        /// Read all <see cref="File"/> entries in root node.
        /// </summary>
        public void Read(string file)
        {
            Stopwatch sw = Stopwatch.StartNew();

            var rawFiles = new HashSet<File>();
            using (var stream = new FileStream(file, FileMode.Open))
            {
                using (var reader = new BinaryReader(stream))
                {
                    var header = reader.ReadBytes(Marshal.SizeOf<DiskHeaderBlock>()).DeSerialise<DiskHeaderBlock>();
                    BtNode.Read(rawFiles, reader, header.FileInfo.BlockSize, header.FileInfo.BTreeRoot);
                }
            }

            InitialiseFiles(rawFiles);

            log.Info($"Loaded datastore {file} in {sw.ElapsedMilliseconds}ms.");
        }

        protected abstract void InitialiseFiles(HashSet<File> rawFiles);
        protected abstract File GetFile(uint full);

        /// <summary>
        /// Return <see cref="IUnpackable"/>, if not already cached <see cref="IUnpackable"/> will be constructed from the raw <see cref="File"/>.
        /// </summary>
        protected T CacheObject<T>(File file) where T : IUnpackable, new()
        {
            if (objectCache.TryGetValue(file.Entry.Id, out IUnpackable dbObj))
                return (T)dbObj;

            var newDbObj = new T();
            using (var stream = new MemoryStream(file.Data))
            {
                using (var reader = new BinaryReader(stream))
                {
                    newDbObj.UnPack(reader);
                    /*if (stream.Position != stream.Length)
                        throw new InvalidDataException($"Failed to fully parse 0x{file.Entry.Id:X8}!");*/
                }
            }

            objectCache.Add(file.Entry.Id, newDbObj);
            return newDbObj;
        }
    }
}
