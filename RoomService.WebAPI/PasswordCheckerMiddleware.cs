using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using Microsoft.AspNetCore.Builder;
using System.Net.Http;
using System.Linq;

namespace RoomService.WebAPI
{
    public class PasswordCheckerMiddleware
    {
        private const string APIKEYNAME = "passwordKey";

        private readonly HttpClient Client;
        private readonly RequestDelegate _next;
        public PasswordCheckerMiddleware(RequestDelegate next, HttpClient Client)
        {
            this._next = next;
            this.Client = Client;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            //Client.DefaultRequestHeaders.Add("passwordKey", "passwordKey123456789");
            string Value = "passwordKey123456789";

            try
            {
                if (!Client.DefaultRequestHeaders.TryGetValues(APIKEYNAME, out var passwordValue))
                {
                    context.Response.StatusCode = 403;
                    context.Response.ContentType = "application/json";
                    await context.Response.WriteAsync(JsonConvert.SerializeObject(new
                    {
                        statusCode = 403,
                        isSuccess = false,
                        message = "key is not available",
                    }));
                    return;
                }
                if (!Value.Equals(passwordValue.FirstOrDefault()))
                {
                    context.Response.StatusCode = 403;
                    context.Response.ContentType = "application/json";
                    await context.Response.WriteAsync(JsonConvert.SerializeObject(new
                    {
                        statusCode = 403,
                        isSuccess = false,
                        message = "Password Key!",
                    }));
                    return;
                }

                await this._next(context);
            }
            catch (Exception)
            {
                throw;
            }
        }


    }
    public static class PasswordCheckerMiddlewareExtensions
    {
        public static IApplicationBuilder UsePasswordMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<PasswordCheckerMiddleware>();
        }
    }
}
