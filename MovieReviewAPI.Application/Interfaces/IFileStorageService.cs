namespace MovieReviewApi.Application.Interfaces
{
    public interface IFileStorageService {
        Task<string> UploadFileAsync(Stream stream, string fileName, string contentType = null!, string storageProvider = "local", CancellationToken cancellationToken = default);
        Task DeleteFileAsync(Guid movieId, string storageProvider = "local", CancellationToken cancellationToken = default);
        Task<string> UpdateFileAsync(Stream newStream, Guid movieId, string fileName, string storageProvider = "local", CancellationToken cancellationToken = default);

    }

}

