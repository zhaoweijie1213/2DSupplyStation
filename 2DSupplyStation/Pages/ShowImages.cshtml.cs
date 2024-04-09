using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace _2DSupplyStation.Pages
{
    public class ShowImagesModel : PageModel
    {
        private readonly IWebHostEnvironment _environment;

        // 存储图片文件的路径
        public List<string> ImagePaths { get; private set; } = [];

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
            var imagesDir = Path.Combine(_environment.WebRootPath, "images");
            if (Directory.Exists(imagesDir))
            {
                ImagePaths = Directory.EnumerateFiles(imagesDir)
                                      .Where(file => sourceArray.Contains(Path.GetExtension(file)?.ToLower()))
                                      .Select(fileName => "/Images/" + Path.GetFileName(fileName))
                                      .ToList();
            }
            else
            {
                ImagePaths = [];
            }
        }
    }
}
