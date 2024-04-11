using _2DSupplyStation.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
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

            Images = GetImages(product);

            //var imagesDir = Path.Combine(_environment.WebRootPath, "images", product);

            //_logger.LogInformation("OnGet.图片路径:{imagesDir}", imagesDir);

            //if (Directory.Exists(imagesDir))
            //{

            //    Images = Directory.EnumerateFiles(imagesDir)
            //                      .Where(file => sourceArray.Contains(Path.GetExtension(file)?.ToLower()))
            //                      .Select(filePath => new ImageInfo
            //                      {
            //                          FileName = Path.GetFileNameWithoutExtension(filePath),
            //                          FilePath = $"/images/{product}/" + Path.GetFileName(filePath)
            //                      })
            //                      .OrderBy(i => i.FileName)
            //                      .ToList();

            //    _logger.LogInformation("OnGet.图片列表:{images}", JsonConvert.SerializeObject(Images));
            //}
            //else
            //{
            //    Images = new List<ImageInfo>();
            //}
        }

        public List<ImageInfo> GetImages(string product)
        {
            string key = $"data_{product}";
            if (!memoryCache.TryGetValue(key,out List<ImageInfo>? images))
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

                    _logger.LogInformation("OnGet.图片列表:{images}", JsonConvert.SerializeObject(Images));

                    memoryCache.Set(key, images);
                }
            }
            return images!;
        }

    }
}
