using Microsoft.AspNetCore.Http;
using System;
using System.Net;
using System.Threading.Tasks;
using Elmah.Io.AspNetCore.ExceptionFormatters;
using Elmah.Io.AspNetCore;

namespace DevIO.API.Extensions
{
    // Middleware que vai capturar todas as Exceptions nao tratadas e enviar para Elmah
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch(Exception ex)
            {
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        public static async Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            ex.Ship(context);
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        }
    }
}
