using Experiments.DapperRepository.Models;
using System.Collections.Generic;

namespace Experiments.DapperRepository
{
    public interface ISampleRepository
    {
        Sample Get(int id);

        IEnumerable<Sample> Get();
    }
}