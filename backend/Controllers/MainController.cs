using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using GreenDrive.Components;
using GreenDrive.Models;
using Google.Apis.Drive.v2;
using Google.Apis.Drive.v2.Data;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Newtonsoft.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.ObjectPool;

namespace GreenDrive.Controllers
{
    [Route("Api")]
    [ApiController]
    public class MainController : ControllerBase
    {
        public DriveService svc { get; set; }
        public DownloadStorage stor { get; set; }
        public Cache cache { get; set; }

        internal IConfiguration configuration;

        internal GDriveConfiguration gdriveConf;
        public MainController(IConfiguration _configuration, DriveApiService _svc, DownloadStorage _stor, Cache _cache)
        {
            svc = _svc.service;
            stor = _stor;
            cache = _cache;
            configuration = _configuration;
            gdriveConf = configuration.GetSection("GDrive").Get<GDriveConfiguration>();

        }
        [HttpGet("List")]
        public async Task<string> List([FromQuery(Name = "id")] string id)
        {
            List<DriveResult> result;
            if (string.IsNullOrEmpty(id))
            {
                id = gdriveConf.MainFolderId;
            }
            else
            {
                id = Base64.Base64Decode(id);
            }

            if (cache.GetCache(id) != null)
            {
                result = cache.GetCache(id);
            }
            else
            {
                if (gdriveConf.EnableMainFolderCheck && id != gdriveConf.MainFolderId)
                {
                    var isInMain = await ApiBase.IsInMainFolder(svc, id, gdriveConf.MainFolderId);
                    if (!isInMain)
                    {
                        return "Requested item is not child of main folder.";
                    }
                }

                var req = svc.Files.List();
                if (gdriveConf.EnableSharedDrive)
                {
                    req.SupportsTeamDrives = true;
                    req.IncludeItemsFromAllDrives = true;
                    req.Corpora = "drive";
                    req.DriveId = gdriveConf.SharedDriveId;
                }
                req.OrderBy = "folder,title";
                req.MaxResults = 30000;
                req.Q = $"'{id}' in parents and trashed=false";

                var reqResult = await req.ExecuteAsync();
                var filtResult = new List<DriveResult>();
                foreach (var item in reqResult.Items)
                {
                    filtResult.Add(new DriveResult()
                    {
                        Id = Base64.Base64Encode(item.Id),
                        ModTime = item.ModifiedDate.ToString(),
                        Name = item.Title,
                        Size = FormatLength(item.FileSize),
                        MimeType = item.MimeType
                    });
                }
                cache.CreateCache(id, filtResult);
                result = filtResult;
            }

            return JsonConvert.SerializeObject(new
            {
                Items = result
            });
        }

        [HttpGet("Query")]
        public async Task<string> Query([FromQuery(Name = "id")] string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                id = gdriveConf.MainFolderId;
            }
            else
            {
                id = Base64.Base64Decode(id);
            }

            if (gdriveConf.EnableMainFolderCheck && id != gdriveConf.MainFolderId)
            {
                var isInMain = await ApiBase.IsInMainFolder(svc, id, gdriveConf.MainFolderId);
                if (!isInMain)
                {
                    return "Requested item is not child of main folder.";
                }
            }
            var req = svc.Files.Get(id);
            if (gdriveConf.EnableSharedDrive)
            {
                req.SupportsTeamDrives = true;
                req.SupportsAllDrives = true;
            }

            req.AcknowledgeAbuse = true;
            var result = await req.ExecuteAsync();

            return JsonConvert.SerializeObject(new DriveResult()
            {
                Id = Base64.Base64Encode(result.Id),
                ModTime = result.ModifiedDate.ToString(),
                Name = result.Title,
                Size = result.FileSize.ToString(),
                MimeType = result.MimeType
            });
        }
        [HttpGet("CreateToken")]
        public string CreateToken([FromQuery(Name = "id")] string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                return stor.CreateToken(Base64.Base64Decode(id));
            }
            else
            {
                return "";
            }

        }

        internal string FormatLength(long? len)
        {
            string[] sizes = { "B", "KB", "MB", "GB", "TB" };

            int order = 0;
            while (len >= 1024 && order < sizes.Length - 1)
            {
                order++;
                len = len / 1024;
            }
            return String.Format("{0:0.##} {1}", len, sizes[order]);
        }
    }
}
