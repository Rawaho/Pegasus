using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using NLog;

namespace Pegasus.Database
{
    public abstract class DatabaseBase
    {
        private static readonly Logger log = LogManager.GetCurrentClassLogger();

        public struct PreparedStatement
        {
            public PreparedStatementId Id { get; }
            public string Query { get; }
            public List<MySqlDbType> Types { get; }

            public PreparedStatement(PreparedStatementId id, string query, params MySqlDbType[] types)
            {
                Id = id;
                Query = query;
                Types = new List<MySqlDbType>(types);
            }
        }

        private string connectionString;

        private readonly Dictionary<PreparedStatementId, PreparedStatement> preparedStatements =
            new Dictionary<PreparedStatementId, PreparedStatement>();

        public void Initialise(string host, uint port, string user, string password, string database)
        {
            var connectionBuilder = new MySqlConnectionStringBuilder
            {
                Server        = host,
                Port          = port,
                UserID        = user,
                Password      = password,
                Database      = database,
                IgnorePrepare = false,
                Pooling       = true,
                SslMode       = MySqlSslMode.None
            };

            connectionString = connectionBuilder.ToString();

            try
            {
                using (var connection = new MySqlConnection(connectionString))
                    connection.Open();

                log.Info($"Successfully connected to {database} database.");
            }
            catch
            {
                log.Info($"Failed to connect to {database} database!");
                throw;
            }

            InitialisePreparedStatements();
        }

        protected abstract void InitialisePreparedStatements();

        /// <summary>
        /// Cache a prepared statement created from query for later use on the database.
        /// </summary>
        protected void AddPreparedStatement(PreparedStatementId id, string query, params MySqlDbType[] types)
        {
            Debug.Assert(types.Length == query.Count(c => c == '?'));
            Debug.Assert(query.Length != 0);

            try
            {
                using (var connection = new MySqlConnection(connectionString))
                {
                    using (var command = new MySqlCommand(query, connection))
                    {
                        foreach (MySqlDbType type in types)
                            command.Parameters.Add("", type);

                        connection.Open();
                        command.Prepare();

                        preparedStatements.Add(id, new PreparedStatement(id, query, types));
                    }
                }
            }
            catch (Exception exception)
            {
                log.Fatal(exception, $"An exception occured while preparing statement {id}!");
                throw;
            }
        }

        protected (int RowsEffected, long LastInsertedId) ExecutePreparedStatement(PreparedStatementId id, params object[] parameters)
        {
            if (!preparedStatements.TryGetValue(id, out PreparedStatement preparedStatement))
                throw new InvalidPreparedStatementException(id);

            try
            {
                using (var connection = new MySqlConnection(connectionString))
                {
                    using (var command = new MySqlCommand(preparedStatement.Query, connection))
                    {
                        for (int i = 0; i < preparedStatement.Types.Count; i++)
                            command.Parameters.Add("", preparedStatement.Types[i]).Value = parameters[i];

                        connection.Open();
                        return (command.ExecuteNonQuery(), command.LastInsertedId);
                    }
                }
            }
            catch (Exception exception)
            {
                log.Error(exception, $"An exception occured while executing prepared statement {id}!");
                throw;
            }
        }

        /// <summary>
        /// Asynchronous version of ExecutePreparedStatement.
        /// </summary>
        protected async Task<(int RowsEffected, long LastInsertedId)> ExecutePreparedStatementAsync(PreparedStatementId id, params object[] parameters)
        {
            if (!preparedStatements.TryGetValue(id, out PreparedStatement preparedStatement))
                throw new InvalidPreparedStatementException(id);

            try
            {
                using (var connection = new MySqlConnection(connectionString))
                {
                    using (var command = new MySqlCommand(preparedStatement.Query, connection))
                    {
                        for (int i = 0; i < preparedStatement.Types.Count; i++)
                            command.Parameters.Add("", preparedStatement.Types[i]).Value = parameters[i];

                        await connection.OpenAsync();
                        return (await command.ExecuteNonQueryAsync(), command.LastInsertedId);
                    }
                }
            }
            catch (Exception exception)
            {
                log.Error(exception, $"An exception occured while executing prepared statement {id}!");
                throw;
            }
        }

        protected MySqlResult SelectPreparedStatement(PreparedStatementId id, params object[] parameters)
        {
            if (!preparedStatements.TryGetValue(id, out PreparedStatement preparedStatement))
                throw new InvalidPreparedStatementException(id);

            Debug.Assert(parameters.Length == preparedStatement.Types.Count);

            try
            {
                using (var connection = new MySqlConnection(connectionString))
                {
                    using (var command = new MySqlCommand(preparedStatement.Query, connection))
                    {
                        for (int i = 0; i < preparedStatement.Types.Count; i++)
                            command.Parameters.Add("", preparedStatement.Types[i]).Value = parameters[i];

                        connection.Open();
                        using (MySqlDataReader commandReader = command.ExecuteReader())
                            return new MySqlResult(commandReader);
                    }
                }
            }
            catch (Exception exception)
            {
                log.Error(exception, $"An exception occured while selecting prepared statement {id}!");
                throw;
            }
        }

        /// <summary>
        /// Asynchronous version of SelectPreparedStatement.
        /// </summary>
        protected async Task<MySqlResult> SelectPreparedStatementAsync(PreparedStatementId id, params object[] parameters)
        {
            if (!preparedStatements.TryGetValue(id, out PreparedStatement preparedStatement))
                throw new InvalidPreparedStatementException(id);

            Debug.Assert(parameters.Length == preparedStatement.Types.Count);

            try
            {
                using (var connection = new MySqlConnection(connectionString))
                {
                    using (var command = new MySqlCommand(preparedStatement.Query, connection))
                    {
                        for (int i = 0; i < preparedStatement.Types.Count; i++)
                            command.Parameters.Add("", preparedStatement.Types[i]).Value = parameters[i];

                        await connection.OpenAsync();
                        using (MySqlDataReader commandReader = (MySqlDataReader)await command.ExecuteReaderAsync())
                            return new MySqlResult(commandReader);
                    }
                }
            }
            catch (Exception exception)
            {
                log.Error(exception, $"An exception occured while selecting prepared statement {id}!");
                throw;
            }
        }

        /// <summary>
        /// Commit a transaction to the database.
        /// </summary>
        public async Task CommitAsync(DatabaseTransaction transaction)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                await connection.OpenAsync();
                using (MySqlTransaction mySqlTransaction = await connection.BeginTransactionAsync())
                {
                    try
                    {
                        foreach (DatabaseTransaction.Statement statement in transaction)
                        {
                            if (!preparedStatements.TryGetValue(statement.Id, out PreparedStatement preparedStatement))
                                throw new InvalidPreparedStatementException(statement.Id);

                            using (MySqlCommand command = new MySqlCommand(preparedStatement.Query, connection, mySqlTransaction))
                            {
                                for (int i = 0; i < statement.Parameters.Length; i++)
                                    command.Parameters.Add("", preparedStatement.Types[i]).Value = statement.Parameters[i];

                                command.Transaction = mySqlTransaction;
                                await command.ExecuteNonQueryAsync();
                            }
                        }

                        mySqlTransaction.Commit();
                    }
                    catch (Exception exception)
                    {
                        log.Error(exception, "An exception occured while commiting database transaction, a rollback will be performed!");

                        try
                        {
                            // serious problem if rollback also fails
                            mySqlTransaction?.Rollback();
                        }
                        catch (Exception rollbackException)
                        {
                            log.Fatal(rollbackException, "An exception occured while rolling back database transaction!");
                            throw;
                        }

                        throw;
                    }
                }
            }
        }
    }
}
