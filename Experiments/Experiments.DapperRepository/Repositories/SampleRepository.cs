using Dapper;
using Experiments.DapperRepository.Factories;
using Experiments.DapperRepository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Experiments.DapperRepository
{
    public class SampleRepository : ISampleRepository
    {
        private readonly IDbConnectionFactory connectionFactory;

        public SampleRepository(IDbConnectionFactory connectionFactory)
        {
            if (connectionFactory == null) throw new ArgumentNullException(nameof(connectionFactory));
            this.connectionFactory = connectionFactory;
        }

        public IEnumerable<Sample> Get()
        {
            var query = new StringBuilder();
            query.Append("SELECT Id, Name, Description FROM Samples");

            using (var connection = connectionFactory.Create())
            {
                return connection.Query<Sample>(query.ToString()).ToArray();
            }
        }

        public Sample Get(int id)
        {
            var query = new StringBuilder();
            query.Append("SELECT Id, Name, Description FROM Samples WHERE Id = @Id");
            var parameters = new DynamicParameters();
            parameters.Add("@Id", id);

            using (var connection = connectionFactory.Create())
            {
                return connection.Query<Sample>(query.ToString(), parameters).First();
            }
        }
    }
}