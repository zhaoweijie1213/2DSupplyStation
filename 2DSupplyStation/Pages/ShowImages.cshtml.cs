using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace _2DSupplyStation.Pages
{
    public class ShowImagesModel : PageModel
    {
        private readonly IWebHostEnvironment _environment;

        // 存储图片文件的路径
        public List<ImageInfo> Images { get; private set; } = [];

        private static readonly string[] sourceArray = [".jpg", ".jpeg", ".png", ".gif", ".bmp"];

        /// <summary>
        /// 
        /// </summary>
        /// <param name="environment"></param>
        public ShowImagesModel(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        public void OnGet()
        {
            var imagesDir = Path.Combine(_environment.WebRootPath, "Images");
            if (Directory.Exists(imagesDir))
            {
                Images = Directory.EnumerateFiles(imagesDir)
                                  .Where(file => new[] { ".jpg", ".jpeg", ".png", ".gif", ".bmp" }
                                  .Contains(Path.GetExtension(file)?.ToLower()))
                                  .Select(filePath => new ImageInfo
                                  {
                                      FileName = Path.GetFileNameWithoutExtension(filePath),
                                      FilePath = "/Images/" + Path.GetFileName(filePath)
                                  })
                                  .ToList();
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
