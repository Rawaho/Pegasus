using System.Collections.Generic;
using System.Linq;
using Pegasus.DungeonGenerator.Datastore.Object.EnvCell;
using Pegasus.DungeonGenerator.Datastore.Object.LandblockInfo;

namespace Pegasus.DungeonGenerator.Datastore
{
    public class CellDatastore : Datastore
    {
        private readonly Dictionary<ushort /*landblock*/, HashSet<File>> landBlockFiles = new Dictionary<ushort, HashSet<File>>();
        private List<ushort> landBlockInfoIds = new List<ushort>();

        /// <summary>
        /// Sort and store all <see cref="File"/> entries by landblock.
        /// </summary>
        protected override void InitialiseFiles(HashSet<File> rawFiles)
        {
            foreach (File file in rawFiles)
            {
                ushort landBlockId = (ushort)(file.Entry.Id >> 16);
                if (!landBlockFiles.ContainsKey(landBlockId))
                    landBlockFiles.Add(landBlockId, new HashSet<File>());

                landBlockFiles[landBlockId].Add(file);
            }

            landBlockInfoIds = rawFiles
                .Where(f => (f.Entry.Id & 0x0000FFFF) == 0x0000FFFE)
                .Select(f => (ushort)(f.Entry.Id >> 16))
                .ToList();
        }

        protected override File GetFile(uint full)
        {
            File file = landBlockFiles[(ushort)(full >> 16)].SingleOrDefault(f => f.Entry.Id == full);
            if (file == null)
                throw new DatastoreFileNotFoundException(full);

            return file;
        }

        public IEnumerable<CLandBlockInfo> GetLandBlockInfo()
        {
            foreach (ushort landBlockId in landBlockInfoIds)
                yield return GetLandBlockInfo(landBlockId);
        }

        /// <summary>
        /// Return <see cref="CLandBlockInfo"/>, if not already cached <see cref="CLandBlockInfo"/> will be constructed from the raw <see cref="File"/>.
        /// </summary>
        public CLandBlockInfo GetLandBlockInfo(ushort landBlockId)
        {
            File landBlockInfoFile = landBlockFiles[landBlockId].SingleOrDefault(f => f.Entry.Id == (((uint)landBlockId << 16) | 0xFFFE));
            if (landBlockInfoFile == null)
                return null;

            CLandBlockInfo landBlockInfo = CacheObject<CLandBlockInfo>(landBlockInfoFile);
            for (uint i = 0; i < landBlockInfo.Cells.Length; i++)
            {
                File envCellFile = GetFile(((uint)landBlockId << 16) | (0x0100 + i));
                landBlockInfo.Cells[i] = CacheObject<CEnvCell>(envCellFile);
            }

            return landBlockInfo;
        }
    }
}
