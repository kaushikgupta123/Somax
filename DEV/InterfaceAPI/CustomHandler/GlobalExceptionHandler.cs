using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.ExceptionHandling;
using System.Web.Http.Results;

namespace InterfaceAPI.CustomHandler
{
  public class GlobalExceptionHandler : ExceptionHandler
  {
    public async override Task HandleAsync(ExceptionHandlerContext context, CancellationToken cancellationToken)
    {
      // Access Exception using context.Exception;  
      string errorMessage = "An unexpected error occured";
      if (context.Exception.InnerException == null)
      {
        errorMessage = context.Exception.Message;
      }
      else
      {
        errorMessage = context.Exception.InnerException.Message;
      }
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