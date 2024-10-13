using _2DSupplyStation.Models;
using _2DSupplyStation.Pages;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using QYQ.Base.Common.ApiResult;
using QYQ.Base.Common.Extension;
using QYQ.Base.Common.IOCExtensions;
using System;
using System.Net;

namespace _2DSupplyStation.Services
{
    /// <summary>
    /// 图片服务
    /// </summary>
    public class ImagesService(ILogger<ImagesService> logger, IWebHostEnvironment environment, IMemoryCache memoryCache,IConfiguration configuration, IOptionsMonitor<List<MenuConfig>> menus) : ITransientDependency
    {

        // 存储图片文件的路径
        public List<ImageInfo> Images { get; private set; } = [];

        private static readonly string[] sourceArray = [".jpg", ".jpeg", ".png", ".gif", ".bmp"];

        /// <summary>
        /// 获取菜单
        /// </summary>
        /// <returns></returns>
        public ApiResult<List<MenuConfig>> Menus()
        {
            ApiResult<List<MenuConfig>> result = new();
            string imagesPath = Path.Combine(environment.WebRootPath, "images");
            DirectoryInfo imagesDirInfo = new(imagesPath);
            // 获取所有子目录
            var hiddenDirectories = imagesDirInfo.GetDirectories();
            var list = menus.CurrentValue.Select(i => new MenuConfig()
            {
                Name = i.Name,
                Path = hiddenDirectories.FirstOrDefault(x => x.Name.StartsWith(i.Path))?.Name ?? i.Path
            }).ToList();
            return result.SetRsult(ApiResultCode.Success, list);
        }

        /// <summary>
        /// 获取图片
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        public Task<List<ImageInfo>> GetImagesAsync(string product, int pageNum = 1, int pageSize = 10)
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
                                          //.OrderBy(i => i.FileName)
                                          .ToList();

                        logger.LogInformation("OnGet.图片列表:{images}", JsonConvert.SerializeObject(images));

                        memoryCache.Set(key, images, TimeSpan.FromMinutes(1));
                    }
                }
                var list = images!.Skip((pageNum - 1) * pageSize).Take(pageSize).ToList();
                return Task.FromResult(list);
            }
            catch (Exception e)
            {
                logger.LogError("GetImages:{message}\r\n{StackTrace}", e.Message, e.StackTrace);
            }
            return Task.FromResult(new List<ImageInfo>());
        }

        /// <summary>
        /// 获取隐藏款图片
        /// </summary>
        /// <returns></returns>
        public Task<List<ImageInfo>> GetHiddenImagesAsync(int pageNum = 1, int pageSize = 10)
        {
            try
            {
                string imagesPath = Path.Combine(environment.WebRootPath, "images");
                // 获取所有子目录
                // 使用 DirectoryInfo 获取以 "Hidden" 开头的子目录
                DirectoryInfo imagesDirInfo = new (imagesPath);
                // 过滤以 "Hidden" 开头的子目录
                var hiddenDirectory = imagesDirInfo.GetDirectories("Hidden*").Select(dir =>
                    dir.Name).First();
                string key = $"data_{hiddenDirectory}";
                //获取缓存
                if (!memoryCache.TryGetValue(key, out List<ImageInfo>? images))
                {
                    string domain = configuration.GetSection("Domain").Get<string>() ?? "";
                    string imageDir = Path.Combine(imagesPath, hiddenDirectory);
                    if (Directory.Exists(imageDir))
                    {

                        images = Directory.EnumerateFiles(imageDir)
                                          .Where(file => sourceArray.Contains(Path.GetExtension(file)?.ToLower()))
                                          .Select(filePath => new ImageInfo
                                          {
                                              FileName = Path.GetFileNameWithoutExtension(filePath),
                                              FilePath = $"{domain}/images/{hiddenDirectory}/" + Path.GetFileName(filePath)
                                          })
                                          //.OrderBy(i => i.FileName)
                                          .ToList();

                        logger.LogInformation("GetHiddenImagesAsync.图片列表:{images}", JsonConvert.SerializeObject(images));

                        memoryCache.Set(key, images, TimeSpan.FromMinutes(1));
                    }
                }
                var list = images!.Skip((pageNum - 1) * pageSize).Take(pageSize).ToList();
                return Task.FromResult(list);
            }
            catch (Exception e)
            {
                logger.BaseErrorLog("GetHiddenImagesAsync", e);
            }
            return Task.FromResult(new List<ImageInfo>());
        }

    }
}
