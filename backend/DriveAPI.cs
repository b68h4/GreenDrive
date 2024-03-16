using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Auth.OAuth2.Requests;
using Google.Apis.Drive.v2;
using Google.Apis.Drive.v2.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using GreenDrive.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace GreenDrive
{
    public class DriveApiService
    {
        public string[] Scopes = { DriveService.Scope.DriveReadonly, DriveService.Scope.Drive };

        private ClientSecrets secrets { get; set; }

        public DriveService service { get; set; }

        public GoogleAuthorizationCodeFlow flow { get; set; }

        private GDriveConfiguration gDriveConf { get; set; }

        public string OneTimeToken { get; set; }

        public bool FirstTime { get; set; } = true;

        public DriveApiService(IConfiguration configuration)
        {
            gDriveConf = configuration.GetSection("GDrive").Get<GDriveConfiguration>();

            secrets = new ClientSecrets()
            {
                ClientId = gDriveConf.ClientId,
                ClientSecret = gDriveConf.ClientSecret
            };
            flow = new GoogleAuthorizationCodeFlow(new GoogleAuthorizationCodeFlow.Initializer
            {
                ClientSecrets = secrets,
                Scopes = Scopes,
                DataStore = new FileDataStore(gDriveConf.AuthFolder, true),
                Prompt = "consent" // This forces the consent screen to appear every auth time and get a refresh token
            });
            OneTimeToken = Guid.NewGuid().ToString("n");
            if (CheckCache() && FirstTime)
            {
                var tokenResp = flow.LoadTokenAsync(gDriveConf.AppName, CancellationToken.None).Result;
                SetupService(new UserCredential(flow, gDriveConf.AppName, tokenResp));
                FirstTime = false;
            }
            else
            {
                Console.WriteLine($"Please go to the following link for authorization: http://{{yourdomain}}/Api/Auth?token={OneTimeToken}");
            }
        }

        public void SetupService(UserCredential credential)
        {
            service = new DriveService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = gDriveConf.AppName,
            });
        }

        public bool CheckCache()
        {
            return System.IO.File.Exists(Path.Combine(Environment.CurrentDirectory, gDriveConf.AuthFolder, "Google.Apis.Auth.OAuth2.Responses.TokenResponse-" + gDriveConf.AppName));
        }
    }
}
