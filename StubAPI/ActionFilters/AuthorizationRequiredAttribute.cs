using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using System.Net.Http;
using System.Net;
using StubAPI.BAL;
namespace StubAPI.ActionFilters
{
    public class AuthorizationRequiredAttribute : ActionFilterAttribute
    {
        private const string Token = "Token";
        public string TokenId { get; set; }

        public override void OnActionExecuting(HttpActionContext filterContext)
        {
            //  Get API key provider

            
            TokenServices provider = new TokenServices();
                //filterContext.ControllerContext.Configuration
            //.DependencyResolver.GetService(typeof(ITokenServices)) as ITokenServices;

            if (filterContext.Request.Headers.Contains(Token))
            {
                var tokenValue = filterContext.Request.Headers.GetValues(Token).First();
                TokenId = tokenValue;
                if (tokenValue == string.Empty)
                {
                    var responseMessage = new HttpResponseMessage(HttpStatusCode.Unauthorized) { ReasonPhrase = "Invalid Token" };
                    filterContext.Response = responseMessage;
                }
                // Validate Token
               else if ( !provider.ValidateToken(tokenValue))
                {
                    var responseMessage = new HttpResponseMessage(HttpStatusCode.Unauthorized) { ReasonPhrase = "Invalid request/token expired." };
                    filterContext.Response = responseMessage;
                }
               
            }
            else
            {
                var responseMessage = new HttpResponseMessage(HttpStatusCode.Unauthorized) { ReasonPhrase = "Invalid key." };
                filterContext.Response = responseMessage;
            }

            base.OnActionExecuting(filterContext);

        }
    }
}