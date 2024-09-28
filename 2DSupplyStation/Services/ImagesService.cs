using _2DSupplyStation.Models;
using _2DSupplyStation.Pages;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using QYQ.Base.Common.IOCExtensions;
using System;
using System.Net;

namespace _2DSupplyStation.Services
{
    /// <summary>
    /// 图片服务
    /// </summary>
    public class ImagesService(ILogger<ImagesService> logger, IWebHostEnvironment environment, IMemoryCache memoryCache,IConfiguration configuration) : ITransientDependency
    {

        // 存储图片文件的路径
        public List<ImageInfo> Images { get; private set; } = [];

        private static readonly string[] sourceArray = [".jpg", ".jpeg", ".png", ".gif", ".bmp"];

        /// <summary>
        /// 获取图片
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        public Task<List<ImageInfo>> GetImagesAsync(string product)
        {
            try
            {
                string key = $"data_{product}";
                if (!memoryCache.TryGetValue(key, out List<ImageInfo>? images))
                {
                    var imagesDir = Path.Combine(environment.WebRootPath, "images", product);

                    logger.LogInformation("OnGet.图片路径:{imagesDir}", imagesDir);

                    string domain = configuration.GetSection("Domain").Get<string>() ?? "";
                    if (Directory.Exists(imagesDir))
                    {

                        images = Directory.EnumerateFiles(imagesDir)
                                          .Where(file => sourceArray.Contains(Path.GetExtension(file)?.ToLower()))
                                          .Select(filePath => new ImageInfo
                                          {
                                              FileName = Path.GetFileNameWithoutExtension(filePath),
                                              FilePath = $"{domain}/images/{product}/" + Path.GetFileName(filePath)
                                          })
                                          .OrderBy(i => i.FileName)
                                          .ToList();

                        logger.LogInformation("OnGet.图片列表:{images}", JsonConvert.SerializeObject(images));

                        memoryCache.Set(key, images, TimeSpan.FromMinutes(1));
                    }
                }

                return Task.FromResult(images!);
            }
            catch (Exception e)
            {
                logger.LogError("GetImages:{message}\r\n{StackTrace}", e.Message, e.StackTrace);
            }
            return Task.FromResult(new List<ImageInfo>());
        }

    }
}
