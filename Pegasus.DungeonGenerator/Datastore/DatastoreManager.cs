using System;
using System.Collections.Generic;
using System.Linq;
using NLog;

namespace Pegasus.DungeonGenerator.Datastore
{
    public static class DatastoreManager
    {
        private static readonly Logger log = LogManager.GetCurrentClassLogger();

        public static CellDatastore CellDatastore { get; private set; }
        public static PortalDatastore PortalDataStore { get; } = new PortalDatastore();

        private static readonly Dictionary<DbType, DbTypeAttribute> dbTypes = new Dictionary<DbType, DbTypeAttribute>();

        public static void Initialise()
        {
            log.Info("Loading datastores...");

            // cache attributes for database object types
            foreach (DbType dbType in Enum.GetValues(typeof(DbType)))
            {
                DbTypeAttribute attribute = dbType.GetAttributeOfType<DbTypeAttribute>();
                if (attribute == null)
                    continue;

                dbTypes.Add(dbType, attribute);
            }

            CellDatastore = new CellDatastore();
            try
            {
                PortalDataStore.Read("client_portal.dat");
                CellDatastore.Read("client_cell_1.dat");
            }
            catch (Exception exception)
            {
                log.Fatal(exception);
                throw;
            }
        }

        /// <summary>
        /// Return <see cref="DbType"/> for full file identifier.
        /// </summary>
        public static DbType GetDbType(uint full)
        {
            return dbTypes.SingleOrDefault(type => type.Value.Base <= full && type.Value.Top >= full).Key;
        }
    }
}
