﻿using _2DSupplyStation.Models;
using _2DSupplyStation.Services;
using Asp.Versioning;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using QYQ.Base.Common.ApiResult;
using System.Net;

namespace _2DSupplyStation.Controller
{
    [Route("/api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("1")]
    [ApiExplorerSettings(GroupName = "v1")]
    public class ImagesController (ILogger<ImagesController> logger, ImagesService imagesService) : ControllerBase
    {
        /// <summary>
        /// 列表
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        [HttpGet("List")]
        public async Task<ApiResult<List<ImageInfo>>> List(string product)
        {
            ApiResult<List<ImageInfo>> result = new();
            string ipAddress = "";
            // 获取IP地址
            if (HttpContext.Request.Headers.TryGetValue("X-Forwarded-For", out Microsoft.Extensions.Primitives.StringValues value))
            {
                ipAddress = value.ToString();
            }
            else
            {
                IPAddress? remoteIpAddress = HttpContext.Connection.RemoteIpAddress;
                if (remoteIpAddress != null)
                {
                    ipAddress = remoteIpAddress.ToString();
                }

            }
            // 获取User Agent
            string userAgent = HttpContext.Request.Headers["User-Agent"].ToString();
            // 使用获取的信息
            // 例如，将它们返回在视图中或进行某些处理
            logger.LogInformation("Product:{product},IP Address: {ipAddress}, User Agent: {userAgent}", product, ipAddress, userAgent);

            var images = await imagesService.GetImagesAsync(product);
            result.SetRsult(ApiResultCode.Success,images);
            return result;
        }
    }
}