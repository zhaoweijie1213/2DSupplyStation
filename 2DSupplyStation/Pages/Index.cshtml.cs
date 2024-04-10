using _2DSupplyStation.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace _2DSupplyStation.Pages
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="environment"></param>
    public class IndexModel(ILogger<IndexModel> logger, IWebHostEnvironment environment) : PageModel
    {
        private readonly ILogger<IndexModel> _logger = logger;

        private readonly IWebHostEnvironment _environment = environment;

        // 存储图片文件的路径
        public List<ImageInfo> Images { get; private set; } = [];

        private static readonly string[] sourceArray = [".jpg", ".jpeg", ".png", ".gif", ".bmp"];

        public void OnGet(string product = "normal")
        {
            var imagesDir = Path.Combine(_environment.WebRootPath, "images", product);

            _logger.LogInformation("OnGet.图片路径:{imagesDir}", imagesDir);

            if (Directory.Exists(imagesDir))
            {

                Images = Directory.EnumerateFiles(imagesDir)
                                  .Where(file => sourceArray.Contains(Path.GetExtension(file)?.ToLower()))
                                  .Select(filePath => new ImageInfo
                                  {
                                      FileName = Path.GetFileNameWithoutExtension(filePath),
                                      FilePath = $"/images/{product}/" + Path.GetFileName(filePath)
                                  })
                                  .OrderBy(i => i.FileName)
                                  .ToList();

                _logger.LogInformation("OnGet.图片列表:{images}", JsonConvert.SerializeObject(Images));
            }
            else
            {
                Images = new List<ImageInfo>();
            }
        }
    }
}
