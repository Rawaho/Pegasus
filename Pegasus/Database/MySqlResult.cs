using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Pegasus.Database
{
    public class MySqlResult : DataTable, IEnumerable<DataRow>
    {
        public uint Count { get; }
        public bool HasRows => Count > 0;
        public DataRow First => HasRows ? Rows[0] : null;

        public IEnumerator<DataRow> GetEnumerator()
        {
            return Rows.Cast<DataRow>().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public MySqlResult(IDataReader reader)
        {
            for (int i = 0; i < reader.FieldCount; i++)
                Columns.Add(reader.GetName(i), reader.GetFieldType(i));

            while (reader.Read())
            {
                DataRow row = NewRow();
                for (int i = 0; i < reader.FieldCount; i++)
                    row[i] = reader.GetValue(i);

                Rows.Add(row);
                Count++;
            }
        }
    }
}
