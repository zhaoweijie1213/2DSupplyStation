using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace _2DSupplyStation.Pages
{
    public class ShowImagesModel : PageModel
    {
        private readonly ILogger<ShowImagesModel> _logger;

        private readonly IWebHostEnvironment _environment;

        // 存储图片文件的路径
        public List<ImageInfo> Images { get; private set; } = [];

        private static readonly string[] sourceArray = [".jpg", ".jpeg", ".png", ".gif", ".bmp"];

        /// <summary>
        /// 
        /// </summary>
        /// <param name="environment"></param>
        public ShowImagesModel(ILogger<ShowImagesModel> logger,IWebHostEnvironment environment)
        {
            _logger = logger;
            _environment = environment;
        }

        public void OnGet()
        {
            var imagesDir = Path.Combine(_environment.WebRootPath, "images");

            _logger.LogInformation("OnGet.图片路径:{imagesDir}", imagesDir);

            // 尝试创建测试文件
            var testFilePath = Path.Combine(imagesDir, "test.txt");
            System.IO.File.WriteAllText(testFilePath, "This is a test file.");

            _logger.LogInformation($"测试文件已创建: {testFilePath}");

            if (Directory.Exists(imagesDir))
            {

                Images = Directory.EnumerateFiles(imagesDir)
                                  .Where(file => new[] { ".jpg", ".jpeg", ".png", ".gif", ".bmp" }
                                  .Contains(Path.GetExtension(file)?.ToLower()))
                                  .Select(filePath => new ImageInfo
                                  {
                                      FileName = Path.GetFileNameWithoutExtension(filePath),
                                      FilePath = "/images/" + Path.GetFileName(filePath)
                                  })
                                  .ToList();

                _logger.LogInformation("OnGet.图片列表:{images}", JsonConvert.SerializeObject(Images));
            }
            else
            {
                Images = new List<ImageInfo>();
            }
        }
    }

    public class ImageInfo
    {
        public string FileName { get; set; } // 不包含扩展名的文件名
        public string FilePath { get; set; } // 图片的相对路径
    }

}
