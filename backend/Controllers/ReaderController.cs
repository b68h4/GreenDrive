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
    public class ReaderController : ControllerBase
    {
        public DriveService drive { get; set; }
        public ReaderController(DriveApiService _svc)
        {
            drive = _svc.service;
        }
        [HttpGet, DisableRequestSizeLimit]
        public async Task<ActionResult> ReaderBridge([FromQuery(Name = "data")] string fileid)
        {
            if (!string.IsNullOrEmpty(fileid))
            {
                var decoded = Base64.Base64Decode(fileid);
                var meta = await ApiBase.GetFileMeta(drive, decoded);
                Logger.WriteLine("ReaderPDF", Request.Method, HttpContext.Connection.RemoteIpAddress.ToString(), $"Client requested the pdf document. ID: {decoded}");

                var result = await ApiBase.SendRequest(drive, decoded, null);
                var resp = result.HttpResponse;
                if (!result.Error)
                {
                    string conlength = resp.Content.Headers.ContentLength.ToString();

                    if (!string.IsNullOrEmpty(conlength))
                    {
                        Response.Headers.Add("Content-Length", conlength);
                    }
                    return File(await resp.Content.ReadAsStreamAsync(), "application/pdf", meta.OriginalFilename, true);
                }
                else
                {

                    Response.StatusCode = 403;
                    return Content(result.ErrorMessage);
                }
            }
            else
            {
                return Content("Error with getting media!, Reason: Wrong Data");
            }
        }
    }
}
