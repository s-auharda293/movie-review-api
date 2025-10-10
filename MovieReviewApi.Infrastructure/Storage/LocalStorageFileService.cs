using System.IO;

namespace MovieReviewApi.Infrastructure.Storage
{

    public class LocalFileStorageService
    {
        private readonly string _basePath = Path.Combine(Directory.GetCurrentDirectory(), "uploads");

        public LocalFileStorageService()
        {
            if (!Directory.Exists(_basePath))
                Directory.CreateDirectory(_basePath);
        }

        public async Task<string> UploadFileAsync(Stream stream, string fileName, CancellationToken cancellationToken = default)
        {
            var uniqueFileName = $"{Guid.NewGuid()}_{fileName}";
            var filePath = Path.Combine(_basePath, uniqueFileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
            {
                await stream.CopyToAsync(fileStream, cancellationToken);
            }

            return $"/uploads/{uniqueFileName}";
        }

        public Task DeleteFileAsync(string fileUrl, CancellationToken cancellationToken = default)
        {
            var filePath = Path.Combine(_basePath, Path.GetFileName(fileUrl));
            if (File.Exists(filePath))
                File.Delete(filePath);

            return Task.CompletedTask;
        }
    }

}
