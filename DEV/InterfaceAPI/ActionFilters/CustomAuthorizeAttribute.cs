using Newtonsoft.Json;
using Business.Authentication;
using Common.Enumerations;
using Data.DataContracts;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Net.Http;
using System.Web.Http;
using DataContracts;

namespace InterfaceAPI.ActionFilters
{
  public class CustomAuthorizeAttribute : System.Web.Http.AuthorizeAttribute
  {


    private readonly string[] allowedroles;
    public string msgcode = String.Empty;
    #region Properties And Global Objects

    public UserData UserData { get; set; }
    DataTable dt = new DataTable();


    #endregion Properties
    public CustomAuthorizeAttribute(params string[] roles)
    {
      this.allowedroles = roles;
    }
    //protected override bool AuthorizeCore(HttpContextBase httpContext)
    //{
    //    bool authorize = false;
    //    //retrieve all the user 
    //    ClaimsPrincipal currentClaimsPrincipal = ClaimsPrincipal.Current;
    //    //=========================================================================================================
    //    StringBuilder sb = new StringBuilder();
    //    Guid LoginSessionID = new Guid(currentClaimsPrincipal.Claims.Where(c => c.Type == "LogInSessionId").Select(c => c.Value).SingleOrDefault());
    //    if (!string.IsNullOrEmpty(LoginSessionID.ToString()))
    //    {
    //        try
    //        {
    //            Guid LogsessionId = Guid.Empty;
    //            LogsessionId = LoginSessionID;
    //            DatabaseKey dbKey = Authentication.GetAdminOnlyKey();
    //            this.UserData = new UserData() { SessionId = LogsessionId, WebSite = WebSiteEnum.Client };
    //            this.UserData.Retrieve(dbKey);
    //            Authentication auth = new Authentication() { UserData = this.UserData };
    //            auth.UserData.LoginAuditing.CreateDate = DateTime.Now;
    //            auth.VerifyCurrentUser();
    //            if (auth.IsAuthenticated)
    //            {
    //            Data.DataContracts.Personnel personnel = new Data.DataContracts.Personnel()
    //            {
    //                ClientId = UserData.DatabaseKey.Client.ClientId,
    //                UserInfoId = UserData.DatabaseKey.User.UserInfoId
    //            };
    //            personnel.Retrieve(UserData.DatabaseKey);

    //            }
    //        }
    //        catch (Exception ex) { }
    //    }

    //    return authorize;
    //}

    public override void OnAuthorization(
       System.Web.Http.Controllers.HttpActionContext actionContext)
    {
      base.OnAuthorization(actionContext);
      if (actionContext.Request.Headers.GetValues("Authorization") != null)
      {
        // get value from header
        string authenticationToken = Convert.ToString(
          actionContext.Request.Headers.GetValues("Authorization").FirstOrDefault()).Replace("Bearer ", "");
        //authenticationTokenPersistant
        //HttpCookie myCookie = new HttpCookie("access_token");
        //myCookie = HttpContext.Current.Request.Cookies["access_token"];
        //var authenticationTokenPersistant = myCookie.Value;
        // it is saved in some data store
        // i will compare the authenticationToken sent by client with
        // authenticationToken persist in database against specific user, and act accordingly
        //if ((authenticationTokenPersistant != authenticationToken) && (CheckStatus(actionContext.ActionDescriptor.ActionName)==false))
        //checkstatus function will check the Api enability Accordinly Block and unblock the API
        //string act = actionContext.ControllerContext.ControllerDescriptor.ControllerName;
        if (CheckStatus(actionContext.ActionDescriptor.ActionName) == false)
        {
          HttpContext.Current.Response.AddHeader("authenticationToken", authenticationToken);
          HttpContext.Current.Response.AddHeader("AuthenticationStatus", "NotAuthorized");
          //if (msgcode == "Forbidden")
          //  actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Forbidden);
          //else
          //{ 
          //    actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
          //}
          HandleUnauthorizedRequest(actionContext);
        }
        else
        {
          HttpContext.Current.Response.AddHeader("authenticationToken", authenticationToken);
          HttpContext.Current.Response.AddHeader("AuthenticationStatus", "Authorized");
          //return;
        }
      }
      //actionContext.Response = 
      //  actionContext.Request.CreateResponse(HttpStatusCode.ExpectationFailed);
      //actionContext.Response.ReasonPhrase = "Please provide valid inputs";
    }
    protected override void HandleUnauthorizedRequest(System.Web.Http.Controllers.HttpActionContext actionContext)
    {
      var response = actionContext.Request.CreateResponse<MyError>(new MyError() { Message = msgcode });
      if (msgcode == "Forbidden")
        response.StatusCode = HttpStatusCode.Forbidden;
      else
        response.StatusCode = HttpStatusCode.Unauthorized;
      actionContext.Response = response;
    }

    public bool CheckStatus(string p)
    {
      //set the mechanism for user respects
      ClaimsPrincipal currentClaimsPrincipal = ClaimsPrincipal.Current;
      if (currentClaimsPrincipal.Claims.Where(c => c.Type == "LogInSessionId").Select(c => c.Value).SingleOrDefault() == null)
      {
        msgcode = "Authorization has been denied for this request";
        return false;
      }

      Guid LogsessionId = new Guid(currentClaimsPrincipal.Claims.Where(c => c.Type == "LogInSessionId").Select(c => c.Value).SingleOrDefault());
      //LogsessionId = new Guid(PartObject.TokenID);
      DatabaseKey dbKey = Authentication.GetAdminOnlyKey();
      this.UserData = new UserData() { SessionId = LogsessionId, WebSite = WebSiteEnum.Client };
      this.UserData.Retrieve(dbKey);

      Authentication auth = new Authentication() { UserData = this.UserData };
      auth.UserData.LoginAuditing.CreateDate = DateTime.Now;
      auth.VerifyCurrentUser();
      //Insert Data Into Part Import
      StringBuilder sb = new StringBuilder();
      string Msg = String.Empty;
      if (auth.IsAuthenticated)
      {
        //Data.DataContracts.InterfaceSetup interfaceSetup = new Data.DataContracts.InterfaceSetup();
        //List<Data.DataContracts.InterfaceSetup> list = interfaceSetup.RetriveAll(this.UserData.DatabaseKey).Where(x => x.ClientId == this.UserData.DatabaseKey.Client.ClientId && x.Enabled == true && x.Name == p).ToList();
        //if (list.Count >= 1)
        //{
          return true;
        //}
      }
      msgcode = "Forbidden";
      return false;
    }

  }
  public class MyError
  {
    public string Message { get; set; }
  }
}
