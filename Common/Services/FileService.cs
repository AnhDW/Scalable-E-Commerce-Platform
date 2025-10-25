using Common.Services.IServices;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Webp;
using SixLabors.ImageSharp.Processing;

namespace Common.Services
{
    public class FileService : IFileService
    {
        public string AddAttachment(IFormFile file)
        {
            string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            string fileType = file.ContentType.Split('/')[0] + "s";

            if (file.Length > 0)
            {
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", @fileType, fileName);
                if (!Directory.Exists("wwwroot/" + fileType))
                {
                    Directory.CreateDirectory("wwwroot/" + fileType);
                }
                using (var stream = File.Create(path))
                {
                    file.CopyTo(stream);
                }

                return "/" + fileType + "/" + fileName;
            }

            return null;
        }

        public void DeleteAttachment(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
            {
                Console.WriteLine("URL is null or empty. Cannot delete attachment.");
                return;
            }

            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", url.TrimStart('/'));

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
            else
            {
                Console.WriteLine($"File not found: {filePath}");
            }
        }

        public async Task<string> AddCompressAttachment(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return null;

            string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            string fileType = file.ContentType.Split('/')[0] + "s";
            string uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", fileType);

            // Tạo thư mục nếu chưa có
            if (!Directory.Exists(uploadPath))
            {
                Directory.CreateDirectory(uploadPath);
            }

            string fullPath = Path.Combine(uploadPath, fileName);

            using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            // Nếu là ảnh, nén thành WebP
            if (file.ContentType.StartsWith("image"))
            {
                string compressedPath = Path.Combine(uploadPath, Path.GetFileNameWithoutExtension(fileName) + ".webp");
                CompressImage(fullPath, compressedPath);
                File.Delete(fullPath); // Xóa file gốc
                return $"/{fileType}/{Path.GetFileName(compressedPath)}";
            }

            // Nếu là video, nén thành WebM
            //if (file.ContentType.StartsWith("video"))
            //{
            //    string compressedPath = Path.Combine(uploadPath, Path.GetFileNameWithoutExtension(fileName) + ".webm");
            //    await CompressVideo(fullPath, compressedPath);
            //    File.Delete(fullPath); // Xóa file gốc
            //    return $"/{fileType}/{Path.GetFileName(compressedPath)}";
            //}

            return $"/{fileType}/{fileName}";
        }


        public void CompressImage(string inputPath, string outputPath)
        {
            using (var image = Image.Load(inputPath))
            {
                image.Mutate(x => x.Resize(new ResizeOptions
                {
                    Mode = ResizeMode.Max,
                    Size = new Size(1080, 720) // Giảm kích thước ảnh
                }));

                image.Save(outputPath, new WebpEncoder { Quality = 75 }); // Chất lượng WebP
            }
        }

        //public async Task CompressVideo(string inputPath, string outputPath)
        //{
        //    var ffmpegArgs = $"-i \"{inputPath}\" -c:v libvpx-vp9 -b:v 1M \"{outputPath}\"";
        //    FFmpeg.SetExecutablesPath(ffmpegArgs);
        //    var process = new Process
        //    {
        //        StartInfo = new ProcessStartInfo
        //        {
        //            FileName = "ffmpeg",
        //            Arguments = ffmpegArgs,
        //            RedirectStandardOutput = true,
        //            RedirectStandardError = true,
        //            UseShellExecute = false,
        //            CreateNoWindow = true
        //        }
        //    };

        //    process.Start();
        //    await process.WaitForExitAsync();
        //}

    }
}
