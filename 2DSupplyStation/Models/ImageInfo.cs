namespace _2DSupplyStation.Models
{
    public class ImageInfo
    {
        /// <summary>
        /// 不包含扩展名的文件名
        /// </summary>
        public string FileName { get; set; } = string.Empty;

        /// <summary>
        /// 图片的相对路径
        /// </summary>
        public string FilePath { get; set; } = string.Empty;
    }
}
