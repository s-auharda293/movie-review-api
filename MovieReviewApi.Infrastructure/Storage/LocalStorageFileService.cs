using Microsoft.EntityFrameworkCore;
using MovieReviewApi.Application.Interfaces;
using System.IO;

namespace MovieReviewApi.Infrastructure.Storage
{

    public class LocalFileStorageService
    {
        private readonly IApplicationDbContext _context;
        private readonly string _basePath = Path.Combine(Directory.GetCurrentDirectory(), "uploads");

        public LocalFileStorageService( IApplicationDbContext context)
        {
            _context = context;
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

            return $"https://localhost:7289/uploads/{uniqueFileName}";
        }

        public async Task DeleteFileAsync(Guid movieId,  CancellationToken cancellationToken = default)
        {
            var movie = await _context.Movies.SingleOrDefaultAsync(m => m.Id == movieId,cancellationToken);


            if (movie == null || string.IsNullOrEmpty(movie.Url))
                return;

            var fileUrl = movie.Url;
            var filePath = Path.Combine(_basePath, Path.GetFileName(fileUrl));

            if (File.Exists(filePath))
                File.Delete(filePath);
        }
    }

}
