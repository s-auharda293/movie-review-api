using MovieReviewApi.Application.Interfaces;
using System.IO;

namespace MovieReviewApi.Infrastructure.Storage
{
    public class FileStorageService : IFileStorageService
    {
        private readonly LocalFileStorageService _localStorage;
        private readonly MinioFileStorageService? _minioStorage;

        public FileStorageService(LocalFileStorageService localStorage, MinioFileStorageService? minioStorage = null)
        {
            _localStorage = localStorage;
            _minioStorage = minioStorage;
        }

        public async Task<string> UploadFileAsync(
            Stream stream,
            string fileName,
            string storageProvider = "local",
            CancellationToken cancellationToken = default)
        {
            if (storageProvider.Equals("minio", StringComparison.OrdinalIgnoreCase))
            {
                if (_minioStorage == null)
                    throw new InvalidOperationException("Minio storage is not configured.");

                return await _minioStorage.UploadFileAsync(stream, fileName, cancellationToken);
            }

            // Default is local
            return await _localStorage.UploadFileAsync(stream, fileName, cancellationToken);
        }

        public async Task DeleteFileAsync(Guid movieId, string storageProvider = "local", CancellationToken cancellationToken = default)
        {
            if (storageProvider.Equals("minio", StringComparison.OrdinalIgnoreCase))
            {
                if (_minioStorage == null)
                    throw new InvalidOperationException("Minio storage is not configured.");

                await _minioStorage.DeleteFileAsync(movieId, cancellationToken);
                return;
            }


            await _localStorage.DeleteFileAsync(movieId, cancellationToken);
        }

        public async Task<string> UpdateFileAsync(Stream newStream, Guid movieId, string fileName, string storageProvider = "local", CancellationToken cancellationToken = default)
        {
            await DeleteFileAsync(movieId, storageProvider, cancellationToken);
            return await UploadFileAsync(newStream, fileName, storageProvider: storageProvider, cancellationToken: cancellationToken);
        }
    }
}
