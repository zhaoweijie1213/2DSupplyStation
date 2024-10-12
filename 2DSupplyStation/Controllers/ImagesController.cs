using _2DSupplyStation.AuthenticationExtend;
using _2DSupplyStation.Models;
using _2DSupplyStation.Services;
using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Options;
using QYQ.Base.Common.ApiResult;
using System.Configuration;
using System.Net;

namespace _2DSupplyStation.Controller
{
    [Route("/api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("1")]
    [ApiExplorerSettings(GroupName = "v1")]
    public class ImagesController (ILogger<ImagesController> logger, ImagesService imagesService, IConfiguration configuration) : ControllerBase
    {
        /// <summary>
        /// 获取菜单
        /// </summary>
        /// <returns></returns>
        [HttpGet("Menus")]
        public ApiResult<List<MenuConfig>> Menus(string auth)
        {
            ApiResult<List<MenuConfig>> result = new();
            bool status = ValidateAuth(auth);
            if (status)
            {
                result= imagesService.Menus();
            }
            else
            {
                result.SetRsult(ApiResultCode.Fail, null);
            }
            return result;
        }

        /// <summary>
        /// 列表
        /// </summary>
        /// <param name="product"></param>
        /// <param name="auth"></param>
        /// <param name="pageNum"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [HttpGet("List")]
        public async Task<ApiResult<List<ImageInfo>>> List(string product, string auth, int pageNum = 1, int pageSize = 10)
        {
            ApiResult<List<ImageInfo>> result = new();
            bool status = ValidateAuth(auth);
            if (status)
            {
                if (product.StartsWith("Hidden"))
                {
                    return result.SetRsult(ApiResultCode.ErrorParams, null);
                }
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

                var images = await imagesService.GetImagesAsync(product, pageNum, pageSize);
                result.SetRsult(ApiResultCode.Success, images);
                return result;
            }
            else
            {
                return result.SetRsult(ApiResultCode.Fail, null);
            }
        }

        /// <summary>
        /// 验证
        /// </summary>
        /// <returns></returns>
        private bool ValidateAuth(string auth)
        {
            var code = configuration.GetSection("SeeAuth").Get<string>();
            if (auth == code)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 隐藏款
        /// </summary>
        /// <returns></returns>
        [HttpGet("hid")]
        [Authorize(AuthenticationSchemes = UrlTokenAuthenticationDefaults.AuthenticationScheme)]
        public async Task<ApiResult<List<ImageInfo>>> Hidden(string auth, int pageNum = 1, int pageSize = 10)
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
            logger.LogInformation("Product:{product},IP Address: {ipAddress}, User Agent: {userAgent}", "Hidden", ipAddress, userAgent);

            var images = await imagesService.GetHiddenImagesAsync(pageNum, pageSize);
            result.SetRsult(ApiResultCode.Success, images);
            return result;
        }

    }
}
