using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Auth.OAuth2.Responses;
using Google.Apis.Util.Store;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using GreenDrive;
using GreenDrive.Models;
using System.Threading;
using System;

namespace GreenDrive.Controllers
{
    [Route("Api/[controller]")]
    public class AuthController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly string[] _scopes = { "openid", "email", "profile" };

        private DriveApiService svc;

        private GDriveConfiguration gDriveConf;

        public AuthController(IConfiguration configuration, DriveApiService _svc)
        {
            _configuration = configuration;
            svc = _svc;
            gDriveConf = _configuration.GetSection("GDrive").Get<GDriveConfiguration>();
        }

        [HttpGet]
        public async Task<IActionResult> Login()
        {

            var redirectUri = new Uri($"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}/Api/Auth/Callback");
            Console.WriteLine(redirectUri.ToString());
            var authUri = svc.flow.CreateAuthorizationCodeRequest(redirectUri.ToString()).Build();
            return Redirect(authUri.AbsoluteUri);
        }
        [HttpGet("Callback")]
        public async Task<IActionResult> Callback(string code)
        {
            var redirectUri = new Uri($"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}/Api/Auth/Callback");
            Console.WriteLine(code);
            var token = await svc.flow.ExchangeCodeForTokenAsync(gDriveConf.AppName, code, redirectUri.ToString(), CancellationToken.None);
            var credentials = new UserCredential(svc.flow, gDriveConf.AppName, token);

            svc.SetupService(credentials);

            return Redirect("/Api/List");
        }
    }
}
