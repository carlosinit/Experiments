using Experiments.WebApiOwinHmac.Middlewares;
using Microsoft.Owin;
using Owin;
using System.Web.Http;

[assembly: OwinStartup(typeof(Experiments.WebApiOwinHmac.Startup))]

namespace Experiments.WebApiOwinHmac
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var httpConfiguration = new HttpConfiguration();
            httpConfiguration.MapHttpAttributeRoutes();

            app.UseHmacAuthentication("supersecret");
            app.UseWebApi(httpConfiguration);
        }
    }
}