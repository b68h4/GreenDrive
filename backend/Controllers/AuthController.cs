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
using System.Reflection.Emit;
using Microsoft.AspNetCore.Http.Extensions;

namespace GreenDrive.Controllers
{
    [Route("Api/[controller]")]
    public class AuthController : Controller
    {
        private readonly IConfiguration _configuration;

        private DriveApiService svc;

        private GDriveConfiguration gDriveConf;

        public AuthController(IConfiguration configuration, DriveApiService _svc)
        {
            _configuration = configuration;
            svc = _svc;
            gDriveConf = _configuration.GetSection("GDrive").Get<GDriveConfiguration>();
        }

        [HttpGet]
        public IActionResult Login(string token)
        {
            if (svc.service != null)
            {
                return Redirect("/Api/List");
            }
            if (token != svc.OneTimeToken)
            {
                return BadRequest("Invalid token, please try again using the correct token.");
            }
            var redirectUri = GetRedirectUri();

            var authUri = svc.flow.CreateAuthorizationCodeRequest(redirectUri.ToString()).Build();
            return Redirect(authUri.AbsoluteUri);
        }
        [HttpGet("Callback")]
        public async Task<IActionResult> Callback(string code)
        {
            if (svc.service != null)
            {
                return Redirect("/Api/List");
            }
            var redirectUri = GetRedirectUri();

            var token = await svc.flow.ExchangeCodeForTokenAsync(gDriveConf.AppName, code, redirectUri.ToString(), CancellationToken.None);
            var credentials = new UserCredential(svc.flow, gDriveConf.AppName, token);
            svc.SetupService(credentials);
            if (token.RefreshToken == "")
            {
                return StatusCode(500, "Application does not have a refresh token. Authentication is successful, but the application will run only until the token expires. Please fix this problem.");
            }
            return Redirect("/Api/List");
        }

        internal Uri GetRedirectUri()
        {
            var scheme = HttpContext.Request.Scheme;

            if (scheme == "http" && Request.Headers["X-Forwarded-For"].ToString() != "")
            {
                scheme = "https";
            }
            var redirectUri = new Uri($"{scheme}://{HttpContext.Request.Host}/Api/Auth/Callback");
            return redirectUri;
        }
    }
}
