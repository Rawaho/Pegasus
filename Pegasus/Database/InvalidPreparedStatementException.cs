using System;

namespace Pegasus.Database
{
    public class InvalidPreparedStatementException : Exception
    {
        public InvalidPreparedStatementException(PreparedStatementId id)
            : base($"PreparedStatement {id} doesn't exist!")
        {
        }
    }
}
