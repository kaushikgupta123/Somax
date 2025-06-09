using Microsoft.AspNet.Identity;
using Microsoft.AspNet.SignalR;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.Google;

using Owin;

using System.Security.Claims;
using System.Web.Helpers;
using System.Configuration;

[assembly: OwinStartup(typeof(NotificationHub.Hubs.Startup))]
namespace NotificationHub.Hubs
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var config = ConfigurationManager.AppSettings;

            #region Redis-Config
            string RedisHost = config["RedisHost"].ToString();
            string RedisPort = ConfigurationManager.AppSettings["RedisPort"].ToString();
            string RedisPassword = ConfigurationManager.AppSettings["RedisPassword"].ToString();
            string RedisSsl = ConfigurationManager.AppSettings["RedisSsl"].ToString();
            string RedisAbortConnect = ConfigurationManager.AppSettings["RedisAbortConnect"].ToString();
            string RedisConnectTimeout = ConfigurationManager.AppSettings["RedisConnectTimeout"].ToString();
            string RedisAllowAdmin = ConfigurationManager.AppSettings["RedisAllowAdmin"].ToString();
            string RedisConnString = RedisHost + ":" + RedisPort + ",password=" + RedisPassword + ",ssl=" + RedisSsl + ",abortConnect=" + RedisAbortConnect;
            #endregion
            //GlobalHost.DependencyResolver.UseStackExchangeRedis(new RedisScaleoutConfiguration("somaxv2.redis.cache.windows.net:6380,password=PkK+6d2lkRRE9XRrOnYjboLPzUIkFkjNKjkoAYs0+VE=,ssl=True", "YourServer"));
            //GlobalHost.DependencyResolver.UseStackExchangeRedis("somaxv2.redis.cache.windows.net", 6379, "PkK+6d2lkRRE9XRrOnYjboLPzUIkFkjNKjkoAYs0+VE=", "SomaxV2SignalRchannel");

            //GlobalHost.DependencyResolver.UseRedis(new RedisScaleoutConfiguration("somaxv2.redis.cache.windows.net:6380,password=PkK+6d2lkRRE9XRrOnYjboLPzUIkFkjNKjkoAYs0+VE=,ssl=True,abortConnect=False", "SomaxV2SignalRchannel"));
            //app.MapSignalR();
            #region External-Auth-Config
            string msClient = config["MsClient"].ToString();
            string msSecret = config["MsSecret"].ToString(); ;
            string gmailClient = config["GmailClient"].ToString(); ;
            string gmailSecret = config["GmailSecret"].ToString(); ;
            #endregion

            GlobalHost.DependencyResolver.UseRedis(new RedisScaleoutConfiguration(RedisConnString, "SomaxV2SignalRchannel"));

            //app.MapAzureSignalR(this.GetType().FullName);
            //GlobalHost.HubPipeline.RequireAuthentication();

            // Enable the application to use a cookie to store information for the signed in user
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Login/SomaxLogIn")
            });

            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

            // Enables the application to remember the second login verification factor such as phone or email.
            // Once you check this option, your second step of verification during the login process will be remembered on the device where you logged in from.
            // This is similar to the RememberMe option when you log in.
            app.UseTwoFactorRememberBrowserCookie(DefaultAuthenticationTypes.TwoFactorRememberBrowserCookie);

            // Uncomment the following lines to enable logging in with third party login providers
            app.UseMicrosoftAccountAuthentication(
                clientId: msClient,
                clientSecret: msSecret);

            app.UseGoogleAuthentication(new GoogleOAuth2AuthenticationOptions()
            {
                ClientId = gmailClient,
                ClientSecret = gmailSecret
            });
            AntiForgeryConfig.UniqueClaimTypeIdentifier = ClaimTypes.NameIdentifier;

            app.MapSignalR();
        }
    }
}