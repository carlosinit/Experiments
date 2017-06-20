using CallerWebSite.Http;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace CallerWebSite.Controllers
{
    public class CallerController : ApiController
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public CallerController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IEnumerable<string>> Get()
        {
            using (var client = _httpClientFactory.Create("http://localhost:56370/"))
            {
                var response = await client.GetAsync("api/Home/");
                var result = await response.Content.ReadAsAsync<string[]>();
                return result.AsEnumerable();
            }
        }
    }
}