using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Google.Apis.Drive.v2;
using Google.Apis.Drive.v2.Data;
using Newtonsoft.Json.Linq;

namespace GreenDrive.Components
{
    public static class ApiBase
    {
        public class DriveResponse
        {
            public HttpResponseMessage HttpResponse { get; set; }
            public bool Error { get; set; }
            public string ErrorMessage { get; set; }
        }
        public static async Task<File> GetFileMeta(DriveService drive, string fileid)
        {

            var file = drive.Files.Get(fileid);
            file.SupportsAllDrives = true;
            return await file.ExecuteAsync();
        }

        public static async Task<bool> IsInMainFolder(DriveService drive, string fileId, string homeFolderId)
        {
            var file = await drive.Parents.List(fileId).ExecuteAsync();
            foreach (var parent in file.Items)
            {
                if (parent.Id == homeFolderId)
                {
                    return true;
                }
                if (parent.IsRoot == true)
                {
                    return false;
                }
            }
            var lastParent = file.Items.Last();
            return await IsInMainFolder(drive, lastParent.Id, homeFolderId);
        }
        public static async Task<DriveResponse> SendRequest(DriveService drive, string fileid, string range)
        {
            var cli = new HttpClient(drive.HttpClient.MessageHandler);
            var req = new HttpRequestMessage(HttpMethod.Get, $"https://www.googleapis.com/drive/v3/files/{fileid}?alt=media");


            if (!string.IsNullOrEmpty(range))
            {
                req.Headers.Add("Range", range);
            }
            var resp = await cli.SendAsync(req, HttpCompletionOption.ResponseHeadersRead);
            var ctype = resp.Content.Headers.ContentType.MediaType;

            if (ctype == "application/json")
            {
                var content = await resp.Content.ReadAsStringAsync();
                var message = JObject.Parse(content).SelectToken("error.message");
                return new DriveResponse
                {
                    Error = true,
                    ErrorMessage = message.ToString(),
                    HttpResponse = resp,
                };
            }
            else
            {
                return new DriveResponse
                {
                    Error = false,
                    HttpResponse = resp,
                };
            }
        }
    }
}
