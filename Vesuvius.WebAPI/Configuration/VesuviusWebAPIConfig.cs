using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Net.Http.Headers;

namespace Vesuvius.WebAPI.Configuration
{
    public class VesuviusWebAPIConfig
    {
        public static void  Register(HttpConfiguration config)
        {
            //Web Api Routes
            config.MapHttpAttributeRoutes();

            config.Formatters.JsonFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/html"));

            config.Routes.MapHttpRoute(
                name: "DefaultAPI",
                routeTemplate: "{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
                );
        }
    }
}