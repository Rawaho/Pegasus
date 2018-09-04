using System;
using System.Data;

namespace Pegasus.Database
{
    public static class Extensions
    {
        public static T Read<T>(this DataRow row, string columnName)
        {
            object val = row[columnName];

            if (val is DBNull)
                return default;

            if (typeof(T).IsEnum)
                return (T)Enum.ToObject(typeof(T), Convert.ChangeType(val, typeof(uint)));

            return (T)Convert.ChangeType(val, typeof(T));
        }
    }
}
