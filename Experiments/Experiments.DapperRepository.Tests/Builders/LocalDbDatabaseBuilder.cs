using Experiments.DapperRepository.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;

namespace Experiments.DapperRepository.Tests.Builders
{
    public class LocalDbDatabaseBuilder : IDisposable
    {
        private string databaseFilePath;
        private IDbConnection connection;
        private List<IDbCommand> queries = new List<IDbCommand>();
        private string connectionString;
        private string logFilePath;

        public LocalDbDatabaseBuilder()
        {
            var path = Path.GetTempPath();
            var databaseFileName = Guid.NewGuid().ToString("N") + ".mdf";
            var logFileName = Guid.NewGuid().ToString("N") + ".ldf";

            databaseFilePath = Path.Combine(path, databaseFileName);
            logFilePath = Path.Combine(path, logFileName);

            //Assembly.GetExecutingAssembly().Location

            File.Copy(@"DatabaseTemplate\template.mdf", databaseFilePath);
            File.Copy(@"DatabaseTemplate\template_log.ldf", logFilePath);

            connectionString = $@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename={databaseFilePath};Integrated Security=True;Connect Timeout=30";
            connection = CreateConnection();
        }

        public LocalDbDatabaseBuilder WithSamplesTable(IEnumerable<Sample> samples = null)
        {
            var command = connection.CreateCommand();
            command.CommandText = "CREATE TABLE Samples (" +
                "Id INTEGER PRIMARY KEY IDENTITY NOT NULL," +
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

        public LocalDbDatabaseBuilder Build()
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
            return new SqlConnection(connectionString);
        }

        public void Dispose()
        {
            //if (File.Exists(databaseFilePath))
            //{
            //    File.Delete(databaseFilePath);
            //}
            //if (File.Exists(logFilePath))
            //{
            //    File.Delete(logFilePath);
            //}
        }
    }
}