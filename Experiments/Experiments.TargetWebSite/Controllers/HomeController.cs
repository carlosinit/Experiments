using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;

namespace TargetWebSite.Controllers
{
    public class HomeController : ApiController
    {
        public Task<IEnumerable<string>> Get()
        {
            var strings = new List<string>();
            for (int i = 0; i < 100; i++)
            {
                strings.Add(Guid.NewGuid().ToString("N"));
            }
            return Task.FromResult(strings.AsEnumerable());
        }
    }
}