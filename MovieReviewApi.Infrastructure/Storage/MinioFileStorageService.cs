using Minio;
using Minio.DataModel.Args;


namespace MovieReviewApi.Infrastructure.Storage
{
    public class MinioFileStorageService
    {
        private readonly MinioClient _minio;
        private readonly string _bucketName = "movie-posters";

        public MinioFileStorageService(MinioClient minio)
        {
            _minio = minio;
        }

        public async Task<string> UploadFileAsync(Stream stream, string fileName, string contentType = "application/octet-stream", CancellationToken cancellationToken = default)
        {
            bool found = await _minio.BucketExistsAsync(new BucketExistsArgs().WithBucket(_bucketName), cancellationToken);
            if (!found)
            {
                await _minio.MakeBucketAsync(new MakeBucketArgs().WithBucket(_bucketName), cancellationToken);
            }


            var uniqueFileName = $"{Guid.NewGuid()}_{fileName}";

            await _minio.PutObjectAsync(new PutObjectArgs()
                .WithBucket(_bucketName)
                .WithObject(uniqueFileName)
                .WithStreamData(stream)
                .WithObjectSize(stream.Length)
                .WithContentType(contentType), cancellationToken);

            return $"http://localhost:9000/{_bucketName}/{uniqueFileName}";
        }


        public async Task DeleteFileAsync(string fileUrl, CancellationToken cancellationToken = default)
        {
            var objectName = Path.GetFileName(fileUrl);
            await _minio.RemoveObjectAsync(new RemoveObjectArgs().WithBucket(_bucketName).WithObject(objectName), cancellationToken);
        }
    }

}
