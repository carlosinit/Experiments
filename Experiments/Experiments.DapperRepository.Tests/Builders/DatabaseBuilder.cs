using Experiments.DapperRepository.Models;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;

namespace Experiments.DapperRepository.Tests.Builders
{
    public class DatabaseBuilder
    {
        private const string databaseModuleName = "FakeDatabase";
        private IDbConnection connection;
        private List<IDbCommand> queries = new List<IDbCommand>();

        public DatabaseBuilder()
        {
            connection = new SQLiteConnection("Data Source=:memory:;Version=3;New=True;");
        }

        public DatabaseBuilder WithSamplesTable(IEnumerable<Sample> samples = null)
        {
            var command = connection.CreateCommand();
            command.CommandText = "CREATE TABLE Samples (" +
                "Id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL," +
                "Name VARCHAR(30) NOT NULL," +
                "Description VARCHAR(255) NOT NULL" +
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

        public IDbConnection Build()
        {
            connection.Open();
            foreach (var query in queries)
            {
                query.ExecuteNonQuery();
            }
            return connection;
        }
    }
}