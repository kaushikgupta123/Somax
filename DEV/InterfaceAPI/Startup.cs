using System;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Business.Authentication;
//using Business.Common;
using Common.Constants;
using Common.Enumerations;
using DataContracts;
using Microsoft.Owin;
using Microsoft.Owin.Security.OAuth;
using Newtonsoft.Json.Linq;
using Owin;
using Presentation.Common;

//[assembly: OwinStartup(typeof(InterfaceAPI.Startup))]
[assembly: OwinStartup("InterfaceConfiguration", typeof(InterfaceAPI.Startup))]
namespace InterfaceAPI
{
  public class Startup : OAuthAuthorizationServerProvider
  {
    public void Configuration(IAppBuilder app)
    {
      string sb = "";
      app.UseOAuthBearerAuthentication(
            new OAuthBearerAuthenticationOptions());

      app.UseOAuthAuthorizationServer(
                    new OAuthAuthorizationServerOptions
                    {
                      TokenEndpointPath = new PathString("/Token"),

                      Provider = new OAuthAuthorizationServerProvider()
                      {
                        OnValidateClientAuthentication = async c =>
                        {
                          c.Validated();
                        },

                        OnGrantResourceOwnerCredentials = async c =>
                        {
                          sb = loginValidation(c.UserName, c.Password).ToString();

                          if (sb.Contains("200") == true)
                          {
                                  //--------------Convert data to Json object------------
                                  JObject json = JObject.Parse(sb);
                            string LoginSessionId = json["LoginSessionID"].ToString();
                                  //--------------------------------------------------
                                  Claim claim1 = new Claim(ClaimTypes.Name, c.UserName);
                            Claim[] claims = new Claim[] { claim1 };
                            ClaimsIdentity claimsIdentity =
                                      new ClaimsIdentity(
                                         claims, OAuthDefaults.AuthenticationType);

                            claimsIdentity.AddClaim(new Claim("LogInSessionId", LoginSessionId));
                            c.Validated(claimsIdentity);
                          }
                          else
                          {
                            c.Rejected();
                            c.SetError("Invalid UserId Or Password");
                          }
                        }
                      },

                      AccessTokenExpireTimeSpan = TimeSpan.FromDays(1),
                      AllowInsecureHttp = true
                    });
    }

    public static string loginValidation(string LogInID, string Password)
    {
      StringBuilder sb = new StringBuilder();

      if (!string.IsNullOrEmpty(LogInID.Trim()) && !string.IsNullOrEmpty(Password.Trim()))
      {

        try
        {
          string hashedPwd = Encryption.SHA512Encrypt(LogInID.ToUpper() + Password);

          Authentication auth = new Authentication()
          {
            UserName = LogInID,
            Password = Password,
            website = WebSiteEnum.Client,
            BrowserInfo = HttpContext.Current.Request.Browser.Type + " " + HttpContext.Current.Request.Browser.Version,
            IpAddress = HttpContext.Current.Request.UserHostAddress
          };
          auth.VerifyLogin();

          if (auth.IsAuthenticated)
          {
            // store the newly created session id into cookie
            Cookie.Set(CookiesConstants.SOMAX_USER, auth.SessionId.ToString());

            // store the newly created session id into session                 
            DatabaseKey dbKey = Authentication.GetAdminOnlyKey();
            auth.UserData = new UserData() { SessionId = auth.SessionId, WebSite = WebSiteEnum.Client };
            auth.UserData.Retrieve(dbKey);
            sb.Append("{");
            sb.Append("\"Response\":");
            sb.Append("\"200\",");
            sb.Append("\"AuthenticationToken\":");
            sb.Append("\"" + LogInID.ToString() + "\",");
            sb.Append("\"LoginSessionID\":");
            sb.Append("\"" + auth.SessionId.ToString() + "\",");
            sb.Append("\"DefaultSiteId\":");
            sb.Append("\"" + auth.UserData.DatabaseKey.Personnel.SiteId.ToString() + "\",");

            sb.Append("\"ClientLookupId\":");
            sb.Append("\"" + auth.UserData.DatabaseKey.Personnel.ClientLookupId.ToString() + "\",");

            sb.Append("\"PersonnelId\":");
            sb.Append("\"" + auth.UserData.DatabaseKey.Personnel.PersonnelId.ToString() + "\",");

            sb.Append("\"NameFull\":");
            sb.Append("\"" + auth.UserData.DatabaseKey.Personnel.NameFull.ToString() + "\",");

            sb.Append("\"NameFirst\":");
            sb.Append("\"" + auth.UserData.DatabaseKey.Personnel.NameFirst.ToString() + "\",");

            sb.Append("\"NameMiddle\":");
            sb.Append("\"" + auth.UserData.DatabaseKey.Personnel.NameMiddle.ToString() + "\",");

            sb.Append("\"NameLast\":");
            sb.Append("\"" + auth.UserData.DatabaseKey.Personnel.NameLast.ToString() + "\",");

            sb.Append("\"TabletUser\":");
            if (auth.UserData.DatabaseKey.User != null && auth.UserData.DatabaseKey.User.TabletUser != null)
              sb.Append("\"" + auth.UserData.DatabaseKey.User.TabletUser.ToString() + "\"");
            else
              sb.Append("\"" + "Not Found" + "\"");
            sb.Append("}");
          }
          else
          {

            sb.Append("{");
            sb.Append("\"Response\":");
            sb.Append("\"204\"");
            sb.Append("}");
          }
        }
        catch (Exception ex) { }
      }
      return sb.ToString();
    }
  }
}
