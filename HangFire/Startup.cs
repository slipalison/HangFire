using System;
using Microsoft.Owin;
using Owin;
using System.Web.Http;
using Microsoft.Owin.Security.OAuth;
using HangFire.Provider;
using Hangfire;
using HangFire.Hang;
using Microsoft.Owin.Security.Cookies;

[assembly: OwinStartup(typeof(HangFire.Startup))]

namespace HangFire
{
    public class Startup
    {
        public static void Configuration(IAppBuilder app)
        {
            ConfigureOAuth(app);
            app.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);

            GlobalConfiguration.Configuration.UseSqlServerStorage("hang");
            app.UseHangfireDashboard();
            app.UseHangfireServer();

       

            var config = new HttpConfiguration();
            ConfigurationWebApi(config);

            app.UseWebApi(config);
        }

      

        public static void ConfigurationWebApi(HttpConfiguration config)
        {

            

            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
                );

            //config.SuppressDefaultHostAuthentication();
            config.Filters.Add(new AuthorizeAttribute());
        //    config.Filters.Add(new HostAuthenticationFilter(OAuthDefaults.AuthenticationType));
        }

        public static void ConfigureOAuth(IAppBuilder app)
        {
            var OAutServerOption = new OAuthAuthorizationServerOptions
            {
                AllowInsecureHttp = true,
                TokenEndpointPath = new PathString("/api/security/token"),
                AccessTokenExpireTimeSpan = TimeSpan.FromMinutes(1),
                Provider = new AuthAuthorizationServerProvider()
            };
            app.UseOAuthAuthorizationServer(OAutServerOption);
            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());

         
        }
    }
}
