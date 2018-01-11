using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;
using System.Web.Http;
using Microsoft.Practices.Unity.Mvc;
using Microsoft.Practices.Unity;
using System.Collections.Generic;
using System.Web.Http.Dependencies;

[assembly: OwinStartup(typeof(OwinRequestInfo.Startup))]

namespace OwinRequestInfo
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var container = new UnityContainer();

            container.RegisterType<ILoggerFactory, LoggerFactory>(new PerRequestLifetimeManager());

            var config = new HttpConfiguration();
            config.DependencyResolver = (System.Web.Http.Dependencies.IDependencyResolver)new UnityDependencyResolver(container);
            app.UseWebApi(config);

            //config.Services.Replace()
        }
    }

    public class OwinPerRequestDependencyResolver : System.Web.Http.Dependencies.IDependencyResolver
    {
        public IDependencyScope BeginScope()
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public object GetService(Type serviceType)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            throw new NotImplementedException();
        }
    }
}
