using System.Web.Http;

namespace RewardsAPI
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
               name: "pbApi2",
               routeTemplate: "api/v1/{controller}/{ppc}",
               defaults: new { ppc = "p"}
           );

        
      }
    }

}


