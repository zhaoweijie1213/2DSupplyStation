using _2DSupplyStation.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using System.Net;
using static System.Net.Mime.MediaTypeNames;

namespace _2DSupplyStation.Pages
{
    /// <summary>
    /// 
    /// </summary>
    public class IndexModel(ILogger<IndexModel> logger, IWebHostEnvironment environment, IMemoryCache memoryCache) : PageModel
    {
        private readonly ILogger<IndexModel> _logger = logger;

        private readonly IWebHostEnvironment _environment = environment;

        // 存储图片文件的路径
        public List<ImageInfo> Images { get; private set; } = [];

        private static readonly string[] sourceArray = [".jpg", ".jpeg", ".png", ".gif", ".bmp"];

        public void OnGet(string product = "Normal")
        {
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

            _logger.LogInformation("IP Address: {ipAddress}, User Agent: {userAgent}", ipAddress, userAgent);

            Images = GetImages(product);


        }

        public List<ImageInfo> GetImages(string product)
        {
            try
            {
                string key = $"data_{product}";
                if (!memoryCache.TryGetValue(key, out List<ImageInfo>? images))
                {
                    var imagesDir = Path.Combine(_environment.WebRootPath, "images", product);

                    _logger.LogInformation("OnGet.图片路径:{imagesDir}", imagesDir);

                    if (Directory.Exists(imagesDir))
                    {

                        images = Directory.EnumerateFiles(imagesDir)
                                          .Where(file => sourceArray.Contains(Path.GetExtension(file)?.ToLower()))
                                          .Select(filePath => new ImageInfo
                                          {
                                              FileName = Path.GetFileNameWithoutExtension(filePath),
                                              FilePath = $"/images/{product}/" + Path.GetFileName(filePath)
                                          })
                                          .OrderBy(i => i.FileName)
                                          .ToList();

                        _logger.LogInformation("OnGet.图片列表:{images}", JsonConvert.SerializeObject(images));

                        memoryCache.Set(key, images);
                    }
                }

                return images!;
            }
            catch (Exception e)
            {
                _logger.LogError("GetImages:{message}\r\n{StackTrace}", e.Message, e.StackTrace);
            }

            return [];
        }

    }
}
