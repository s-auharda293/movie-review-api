using MovieReviewApi.Application.Interfaces;

namespace MovieReviewApi.Infrastructure.Storage
{
    public class FileStorageService : IFileStorageService
    {
        private readonly LocalFileStorageService _localStorage;
        //private readonly MinioFileStorageService _minioStorage;

        //public FileStorageService(LocalFileStorageService localStorage, MinioFileStorageService minioStorage)
        //{
        //    _localStorage = localStorage;
        //    _minioStorage = minioStorage;
        //}

        //public async Task<string> UploadFileAsync(Stream stream, string fileName, string contentType = null!, string storageProvider = "local", CancellationToken cancellationToken = default)
        //{
        //    if (storageProvider.Equals("minio", StringComparison.OrdinalIgnoreCase))
        //    {
        //        return await _minioStorage.UploadFileAsync(stream, fileName, contentType, cancellationToken);
        //    }

        //    return await _localStorage.UploadFileAsync(stream, fileName, cancellationToken);
        //}

        //public async Task DeleteFileAsync(string fileUrl, string storageProvider = "local", CancellationToken cancellationToken = default)
        //{
        //    if (storageProvider.Equals("minio", StringComparison.OrdinalIgnoreCase))
        //    {
        //        await _minioStorage.DeleteFileAsync(fileUrl, cancellationToken);
        //        return;
        //    }

        //    await _localStorage.DeleteFileAsync(fileUrl, cancellationToken);
        //}


        //public async Task<string> UpdateFileAsync(Stream newStream, string oldFileUrl, string fileName, string storageProvider = "local", CancellationToken cancellationToken = default)
        //{
        //    await DeleteFileAsync(oldFileUrl, storageProvider, cancellationToken);

        //    return await UploadFileAsync(newStream, fileName, storageProvider: storageProvider, cancellationToken: cancellationToken);
        //}

        public FileStorageService(LocalFileStorageService localStorage)
        {
            _localStorage = localStorage;
        }

        public async Task<string> UploadFileAsync(Stream stream, string fileName, string contentType = null!, string storageProvider = "local", CancellationToken cancellationToken = default)
        {
            return await _localStorage.UploadFileAsync(stream, fileName, cancellationToken);
        }

        public async Task DeleteFileAsync(string fileUrl, string storageProvider = "local", CancellationToken cancellationToken = default)
        {
            await _localStorage.DeleteFileAsync(fileUrl, cancellationToken);
        }


        public async Task<string> UpdateFileAsync(Stream newStream, string oldFileUrl, string fileName, string storageProvider = "local", CancellationToken cancellationToken = default)
        {
            await DeleteFileAsync(oldFileUrl, storageProvider, cancellationToken);

            return await UploadFileAsync(newStream, fileName, storageProvider: storageProvider, cancellationToken: cancellationToken);
        }


    }

}
