using Microsoft.EntityFrameworkCore;
using Minio;
using Minio.DataModel.Args;
using MovieReviewApi.Application.Interfaces;


namespace MovieReviewApi.Infrastructure.Storage
{
    public class MinioFileStorageService
    {
        private readonly IMinioClient _minio;
        private readonly string _bucketName = "movie-posters";
        private readonly IApplicationDbContext _context;

        public MinioFileStorageService(IMinioClient minio, IApplicationDbContext context)
        {
            _minio = minio;
            _context = context;
        }

        public async Task<string> UploadFileAsync(Stream stream, string fileName, CancellationToken cancellationToken = default)
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
                .WithContentType("image/jpeg"), cancellationToken);

            return $"http://localhost:9000/{_bucketName}/{uniqueFileName}";
        }


        public async Task DeleteFileAsync(Guid movieId, CancellationToken cancellationToken = default)
        {
            var movie = await _context.Movies.SingleOrDefaultAsync(m => m.Id == movieId, cancellationToken);


            if (movie == null || string.IsNullOrEmpty(movie.Url))
                return;

            var fileUrl = movie.Url;

            var objectName = Path.GetFileName(fileUrl);
            await _minio.RemoveObjectAsync(new RemoveObjectArgs().WithBucket(_bucketName).WithObject(objectName), cancellationToken);
        }
    }

}
