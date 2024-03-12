using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v2;
using Google.Apis.Drive.v2.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
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

        public DriveApiService(IConfiguration configuration)
        {
            var gdriveConf = configuration.GetSection("GDrive");
            secrets = new ClientSecrets()
            {
                ClientId = gdriveConf.GetValue<string>("ClientId"),
                ClientSecret = gdriveConf.GetValue<string>("ClientSecret")
            };
            var auth = GoogleWebAuthorizationBroker.AuthorizeAsync(secrets, Scopes, gdriveConf.GetValue<string>("AppName"),
                new CancellationToken(), new FileDataStore(gdriveConf.GetValue<string>("AuthFolder"), true), new LocalServerCodeReceiver()).Result;

            service = new DriveService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = auth,
                ApplicationName = gdriveConf.GetValue<string>("AppName"),

            });

        }


    }
}
