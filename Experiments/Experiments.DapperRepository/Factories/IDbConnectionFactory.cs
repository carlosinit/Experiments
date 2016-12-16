using System.Data;

namespace Experiments.DapperRepository.Factories
{
    public interface IDbConnectionFactory
    {
        IDbConnection Create();
    }
}