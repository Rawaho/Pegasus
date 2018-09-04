using System.Collections;
using System.Collections.Generic;

namespace Pegasus.Database
{
    public class DatabaseTransaction : IEnumerable<DatabaseTransaction.Statement>
    {
        public struct Statement
        {
            public PreparedStatementId Id { get; }
            public object[] Parameters { get; }

            public Statement(PreparedStatementId id, params object[] parameters)
            {
                Id         = id;
                Parameters = parameters;
            }
        }

        private readonly List<Statement> statements = new List<Statement>();

        public IEnumerator<Statement> GetEnumerator()
        {
            return statements.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void AddPreparedStatement(PreparedStatementId id, params object[] parameters)
        {
            statements.Add(new Statement(id, parameters));
        }
    }
}
