﻿using System.Net;
using System.Net.Http;
using System.Web.Http.Filters;


using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.ExceptionHandling;
using System.Web.Http.Results;

namespace InterfaceAPI.ActionFilters
{
  public class CustomExceptionFilter : ExceptionFilterAttribute
  {
    public override void OnException(HttpActionExecutedContext actionExecutedContext)
    {
      string exceptionMessage = string.Empty;
      if (actionExecutedContext.Exception.InnerException == null)
      {
        exceptionMessage = actionExecutedContext.Exception.Message;
      }
      else
      {
        exceptionMessage = actionExecutedContext.Exception.InnerException.Message;
      }
      //We can log this exception message to the file or database.  
      var response = new HttpResponseMessage(HttpStatusCode.InternalServerError)
      {
        Content = new StringContent("An unhandled exception was thrown by service."),
        ReasonPhrase = "Internal Server Error.Please Contact your Administrator."
      };
      actionExecutedContext.Response = response;
    }
  }
  public class GlobalExceptionHandler : ExceptionHandler
  {
    
    public async override Task HandleAsync(ExceptionHandlerContext context, CancellationToken cancellationToken)
    {
      // Access Exception using context.Exception;  
      const string errorMessage = "An unexpected error occured";
      var response = context.Request.CreateResponse(HttpStatusCode.InternalServerError,
          new
          {
            Message = errorMessage
          });
      response.Headers.Add("X-Error", errorMessage);
      context.Result = new ResponseMessageResult(response);
    }
  }
}