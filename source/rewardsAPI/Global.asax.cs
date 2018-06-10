using RewardsAPI.filters;
using System;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.SessionState;

namespace RewardsAPI
{
    public class WebApiApplication : System.Web.HttpApplication
    {
            protected void Application_Start()
            {
                FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
                RouteConfig.RegisterRoutes(RouteTable.Routes);
                GlobalConfiguration.Configure(WebApiConfig.Register);
                var config = GlobalConfiguration.Configuration;
                config.Filters.Add(new TokenValidationAttribute());
            }

            public override void Init()
            {
                this.PostAuthenticateRequest += MvcApplication_PostAuthenticateRequest;
                base.Init();
            }

            void MvcApplication_PostAuthenticateRequest(object sender, EventArgs e)
            {
                System.Web.HttpContext.Current.SetSessionStateBehavior(
                    SessionStateBehavior.Required);
            }

     }
}
