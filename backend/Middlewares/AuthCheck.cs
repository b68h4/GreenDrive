using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using GreenDrive.Components;
using Microsoft.Extensions.Configuration;

namespace GreenDrive.Middlewares
{
    public class AuthCheck
    {
        private readonly RequestDelegate _next;
        IConfiguration configuration;

        private DriveApiService svc;
        public AuthCheck(IConfiguration _configuration, DriveApiService _svc, RequestDelegate next)
        {
            _next = next;
            configuration = _configuration;
            svc = _svc;
        }

        public async Task Invoke(HttpContext con)
        {
            if (svc.service == null && !con.Request.Path.Value.StartsWith("/Api/Auth") && !svc.CheckCache())
            {
                con.Response.StatusCode = 403;
                await con.Response.WriteAsync("Unauthorized, please login.");
                return;
            }
            else
            {
                await _next(con);
            }
        }
    }

}
