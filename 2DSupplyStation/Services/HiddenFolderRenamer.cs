using System;

namespace _2DSupplyStation.Services
{
    public class HiddenFolderRenamer(ILogger<HiddenFolderRenamer> logger, IWebHostEnvironment webHostEnvironment) : BackgroundService
    {
        private readonly string _imagesPath = Path.Combine(webHostEnvironment.WebRootPath, "images");
        private readonly ILogger<HiddenFolderRenamer> _logger = logger;
        private readonly TimeSpan _interval = TimeSpan.FromMinutes(5);

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("HiddenFolderRenamer 服务已启动。");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    RenameHiddenFolders();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "重命名隐藏文件夹时发生错误。");
                }

                await Task.Delay(_interval, stoppingToken);
            }

            _logger.LogInformation("HiddenFolderRenamer 服务已停止。");
        }

        /// <summary>
        /// 重命名文件夹
        /// </summary>
        private void RenameHiddenFolders()
        {
            if (!Directory.Exists(_imagesPath))
            {
                _logger.LogWarning("目录不存在：{ImagesPath}", _imagesPath);
                return;
            }

            // 获取子文件夹
            var hiddenDirectories = Directory.GetDirectories(_imagesPath);

            foreach (var dirPath in hiddenDirectories)
            {
                string dirName = Path.GetFileName(dirPath);
                string randomSuffix = GenerateRandomSuffix();
                // 检查是否已经有随机后缀
                if (dirName == randomSuffix)
                {
                    continue; //随机后缀相同，跳过
                }
                string newDirName = $"{dirName}-{randomSuffix}";
                string newDirPath = Path.Combine(_imagesPath, newDirName);

                try
                {
                    Directory.Move(dirPath, newDirPath);
                    _logger.LogInformation("已将文件夹重命名：{OldName} -> {NewName}", dirName, newDirName);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "重命名文件夹 {DirName} 时发生错误。", dirName);
                }
            }
        }

        /// <summary>
        /// 生成随机后缀
        /// </summary>
        /// <returns></returns>
        private static string GenerateRandomSuffix()
        {
            // 生成6位随机字符串作为后缀
            const string chars = "abcdefghijklmnopqrstuvwxyz0123456789";
            var random = new Random();
            var suffix = new char[8];

            for (int i = 0; i < suffix.Length; i++)
            {
                suffix[i] = chars[random.Next(chars.Length)];
            }

            return new string(suffix);
        }
    }
}
