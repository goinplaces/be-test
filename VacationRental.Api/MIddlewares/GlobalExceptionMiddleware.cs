using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using VacationRental.Common.Models;

namespace VacationRental.Api
{
    public class GlobalExceptionMiddleware
    {
        private const string DefaultErrorMessage = "An error occurred.";

        public GlobalExceptionMiddleware()
        {
        }

        public async Task Invoke(HttpContext httpContext)
        {
            httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            var ex = httpContext.Features.Get<IExceptionHandlerFeature>()?.Error;

            if (ex == null)
            {
                return;
            }

            var error = new ErrorInfo();
            error.Message = DefaultErrorMessage;
            error.Detail = ex.Message;
            httpContext.Response.ContentType = "application/json";
            await httpContext.Response.WriteAsync(JsonConvert.SerializeObject(error));
        }
        
    }
}