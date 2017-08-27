using System.Reflection;
using System.Threading.Tasks;
using System.Web.Http;

namespace Experiments.WebApiOwinHmac.Controllers
{
    [RoutePrefix("api/health")]
    [Authorize(Roles = "hmac")]
    public class HealthController : ApiController
    {
        [HttpGet]
        [Route("version")]
        public IHttpActionResult Version()
        {
            return Ok(Assembly.GetExecutingAssembly().GetName().Version.ToString());
        }

        [HttpPost]
        [Route("echo")]        
        public async Task<IHttpActionResult> Echo()
        {
            return Ok(await Request.Content.ReadAsStringAsync());
        }
    }
}