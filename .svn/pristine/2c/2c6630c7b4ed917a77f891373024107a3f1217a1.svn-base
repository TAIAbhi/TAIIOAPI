namespace StubAPI.ExceptionLog
{
    using System.Net;
    using System.Net.Http;
    using System.Web.Http.Filters;
    using System.Data.SqlClient;
    using DAL;
    using StubAPI.BAL;
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

            ActivityLogger.ActivityLog("xyz", "Error:" + exceptionMessage);
            //We can log this exception message
            var response = new HttpResponseMessage(HttpStatusCode.InternalServerError)
            {
                Content = new StringContent("An unhandled exception was thrown by service."),
                ReasonPhrase = "Internal Server Error.Please Contact your Administrator."
            };
            actionExecutedContext.Response = response;
        }

    }
}