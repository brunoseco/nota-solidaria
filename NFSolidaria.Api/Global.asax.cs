using Common.Infrastructure.Cache;
using NFSolidaria.Core.Application.Config;
using System.Web.Http;

namespace NFSolidaria.Core.Api
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            InitMemcached.Run();
            AutoMapperConfigCore.RegisterMappings();
            GlobalConfiguration.Configure(WebApiConfig.Register);
        }
    }
}
