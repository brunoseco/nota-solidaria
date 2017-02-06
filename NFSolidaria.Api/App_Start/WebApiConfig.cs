using NFSolidaria.Application.Config;
using Common.API;
using Newtonsoft.Json;
using System.Web.Http;
using System.Web.Http.Cors;
using NFSolidaria.Core.Application.Config;

namespace NFSolidaria.Core.Api
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            var cors = new EnableCorsAttribute("*", "*", "*");
            config.EnableCors(cors);
            config.MapHttpAttributeRoutes();
            EnableReferenceLoop(config);

            config.Routes.MapHttpRoute(
                name: "GetDataCustom",
                routeTemplate: "api/{controller}/GetDataCustom",
                defaults: new { id = RouteParameter.Optional, action = "GetDataCustom" }
            );

            config.Routes.MapHttpRoute(
                name: "GetDataItem",
                routeTemplate: "api/{controller}/GetDataItem",
                defaults: new { id = RouteParameter.Optional, action = "GetDataItem" }
            );

            config.Routes.MapHttpRoute(
                name: "GetTotalByFilters",
                routeTemplate: "api/{controller}/GetTotalByFilters",
                defaults: new { id = RouteParameter.Optional, action = "GetTotalByFilters" }
            );

            config.Routes.MapHttpRoute(
                name: "GetDataListCustom",
                routeTemplate: "api/{controller}/GetDataListCustom",
                defaults: new { id = RouteParameter.Optional, action = "GetDataListCustom" }
            );

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}/{action}",
                defaults: new { id = RouteParameter.Optional, action = "DefaultAction" }
            );

        }

        private static void EnableReferenceLoop(HttpConfiguration config)
        {
            //config.Formatters.JsonFormatter.SerializerSettings = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };
            //config.Formatters.JsonFormatter.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Serialize;
            //config.Formatters.JsonFormatter.SerializerSettings.PreserveReferencesHandling = Newtonsoft.Json.PreserveReferencesHandling.Objects;
        }

    }
}
