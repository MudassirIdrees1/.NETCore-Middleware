using System.Linq;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace RoomService.WebAPI
{
    public class PasswordCheckerMiddleware
    {

        private readonly RequestDelegate _next;

        public PasswordCheckerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Request.Headers.TryGetValue("passwordKey", out var password))
            {
                if (!IsValidPassword(password))
                {
                    context.Response.StatusCode = 403;
                    return;
                }
                else
                {
                    context.Response.StatusCode = 200;
                }
            }
            else
            {
                context.Response.StatusCode = 403;
                return;
            }

            context.Response.StatusCode = 200;
            await _next(context);
        }

        private bool IsValidPassword(string password)
        {
            if (password == "passwordKey123456789")
            {
                return true;
            }
            else
            {
                return false;
            }
            
        }

    }
}
