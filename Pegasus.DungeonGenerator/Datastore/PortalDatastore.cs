using System.Collections.Generic;
using System.Linq;
using Pegasus.DungeonGenerator.Datastore.Object.Environment;

namespace Pegasus.DungeonGenerator.Datastore
{
    public class PortalDatastore : Datastore
    {
        private readonly Dictionary<DbType, HashSet<File>> files = new Dictionary<DbType, HashSet<File>>();

        /// <summary>
        /// Sort and store all <see cref="File"/> entries by <see cref="DbType"/>.
        /// </summary>
        protected override void InitialiseFiles(HashSet<File> rawFiles)
        {
            foreach (File file in rawFiles)
            {
                DbType type = DatastoreManager.GetDbType(file.Entry.Id);
                if (!files.ContainsKey(type))
                    files.Add(type, new HashSet<File>());

                files[type].Add(file);
            }
        }

        protected override File GetFile(uint full)
        {
            DbType type = DatastoreManager.GetDbType(full);
            if (!files.TryGetValue(type, out HashSet<File> typeFiles))
                throw new DatastoreFileNotFoundException(full);

            return typeFiles.SingleOrDefault(file => file.Id == (full & 0xFFFFFF));
        }

        public CEnvironment GetEnvironment(uint full)
        {
            return CacheObject<CEnvironment>(GetFile(full));
        }
    }
}
