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
    public class PlayerController : ControllerBase
    {
        public DriveService drive { get; set; }
        public PlayerController(DriveApiService _svc)
        {
            drive = _svc.service;
        }
        [HttpGet, DisableRequestSizeLimit]
        public async Task<ActionResult> PlayerBridge([FromQuery(Name = "data")] string fileid)
        {
            if (!string.IsNullOrEmpty(fileid))
            {
                var decoded = Base64.Base64Decode(fileid);
                var meta = await ApiBase.GetFileMeta(drive, decoded);
                Logger.WriteLine("Player", Request.Method, HttpContext.Connection.RemoteIpAddress.ToString(), $"Client requested the video. ID: {decoded}");
                var range = Request.Headers["Range"];
                var result = await ApiBase.SendRequest(drive, decoded, range);
                var resp = result.HttpResponse;
                if (!result.Error)
                {
                    string conlength = resp.Content.Headers.ContentLength.ToString();
                    if (!string.IsNullOrEmpty(range))
                    {
                        Response.StatusCode = 206;
                        string conrange = resp.Content.Headers.GetValues("Content-Range").FirstOrDefault();
                        Response.Headers.Add("Content-Range", conrange);

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
                        return File(await resp.Content.ReadAsStreamAsync(), meta.MimeType, meta.Title, true);
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
                return Content("Error while getting media!, Reason: data blank");
            }
        }
    }
}
