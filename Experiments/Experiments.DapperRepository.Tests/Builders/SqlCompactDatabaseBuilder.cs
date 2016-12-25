using Experiments.DapperRepository.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlServerCe;
using System.IO;
using System.Linq;

namespace Experiments.DapperRepository.Tests.Builders
{
    public class SqlCompactDatabaseBuilder : IDisposable
    {
        private string databaseFile;
        private IDbConnection connection;
        private List<IDbCommand> queries = new List<IDbCommand>();
        private string connectionString;

        public SqlCompactDatabaseBuilder()
        {
            databaseFile = Path.GetTempFileName();
            connectionString = $"Data Source={databaseFile};Persist Security Info=False;";
            connection = CreateConnection();
        }

        public SqlCompactDatabaseBuilder WithSamplesTable(IEnumerable<Sample> samples = null)
        {
            var command = connection.CreateCommand();
            command.CommandText = "CREATE TABLE Samples (" +
                "Id INTEGER PRIMARY KEY IDENTITY NOT NULL," +
                "Name NVARCHAR(30) NOT NULL," +
                "Description NVARCHAR(255) NOT NULL" +
                ")";
            queries.Add(command);

            foreach (var item in samples ?? Enumerable.Empty<Sample>())
            {
                var insertCommand = connection.CreateCommand();
                insertCommand.CommandText = "INSERT INTO Samples (Name, Description) VALUES(@Name, @Description)";
                insertCommand.Parameters.Add(CreateParameter(insertCommand, DbType.String, "@Name", item.Name));
                insertCommand.Parameters.Add(CreateParameter(insertCommand, DbType.String, "@Description", item.Description));
                queries.Add(insertCommand);
            }

            return this;
        }

        private static IDbDataParameter CreateParameter(IDbCommand command, DbType type, string name, object value)
        {
            var nameParameter = command.CreateParameter();
            nameParameter.DbType = type;
            nameParameter.ParameterName = name;
            nameParameter.Value = value;
            return nameParameter;
        }

        public SqlCompactDatabaseBuilder Build()
        {
            using (connection)
            {
                connection.Open();
                foreach (var query in queries)
                {
                    query.ExecuteNonQuery();
                }
            }
            return this;
        }

        public IDbConnection CreateConnection()
        {
            return new SqlCeConnection(connectionString);
        }

        public void Dispose()
        {
            if (File.Exists(databaseFile))
            {
                File.Delete(databaseFile);
            }
        }
    }
}