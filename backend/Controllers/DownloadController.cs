using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using GreenDrive.Components;
using Google.Apis.Drive.v2;
using Newtonsoft.Json.Linq;

namespace GreenDrive.Controllers
{
    [Route("Api/[controller]")]
    [ApiController]
    public class DownloadController : ControllerBase
    {
        public DriveService drive { get; set; }
        public DownloadStorage _svc { get; set; }

        public DownloadController(DownloadStorage stor, DriveApiService _drive)
        {
            _svc = stor;
            drive = _drive.service;
        }
        [HttpGet, DisableRequestSizeLimit]
        public async Task<ActionResult> Download([FromQuery(Name = "token")] string token)
        {
            if (!string.IsNullOrEmpty(token))
            {
                var data = _svc.GetData(token);
                if (data != null && !string.IsNullOrEmpty(data.FileId))
                {
                    Logger.WriteLine("Download", Request.Method, HttpContext.Connection.RemoteIpAddress.ToString(), $"Download requested with {token}.");
                    var meta = await ApiBase.GetFileMeta(drive, data.FileId);
                    var range = Request.Headers["Range"];
                    var result = await ApiBase.SendRequest(drive, data.FileId, range);
                    var resp = result.HttpResponse;
                    if (!result.Error)
                    {
                        string conlength = resp.Content.Headers.ContentLength.ToString();
                        Response.Headers.Add("Content-Disposition", $"attachment; filename=\"{meta.OriginalFilename}\"");
                        if (!string.IsNullOrEmpty(range))
                        {
                            Response.StatusCode = 206;
                            Response.Headers.Add("Range", range.ToString());

                            if (!string.IsNullOrEmpty(conlength))
                            {
                                Response.Headers.Add("Content-Length", conlength);
                            }
                            return new FileStreamResult(await resp.Content.ReadAsStreamAsync(), meta.MimeType) { EnableRangeProcessing = true };
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(conlength))
                            {
                                Response.Headers.Add("Content-Length", conlength);
                            }
                            return File(await resp.Content.ReadAsStreamAsync(), meta.MimeType, meta.OriginalFilename, true);
                        }
                    }
                    else
                    {
                        Response.StatusCode = 403;
                        return Content(result.ErrorMessage);
                    }
                }
                else
                {
                    Response.StatusCode = 403;
                    return Content("Token Invalid.");
                }
            }
            else
            {
                Response.StatusCode = 403;
                return Content("Token Invalid.");
            }
        }
    }
}
