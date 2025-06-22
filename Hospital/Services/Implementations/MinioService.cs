using Hospital.Services.Interfaces;
using Minio.DataModel.Args;
using Minio;
using Minio.ApiEndpoints;
using Minio.DataModel;
using System.Security.AccessControl;

namespace Hospital.Services.Implementations
{
    public class MinioService : IMinioStorageService
    {
        private readonly IMinioClient _minioClient;

        public MinioService(IMinioClient minioClient)
        {
            _minioClient = minioClient;
        }

        public async Task<string> UploadFileAsync(Stream stream, string fileName, string contentType, string bucketName)
        {
            await _minioClient.PutObjectAsync(new PutObjectArgs()
                .WithBucket(bucketName)
                .WithObject(fileName)
                .WithStreamData(stream)
                .WithObjectSize(stream.Length)
                .WithContentType(contentType));

            return $"{bucketName}/{fileName}";
        }

        public async Task<Stream?> GetFileAsync(string fileName, string bucketName)
        {
            var memoryStream = new MemoryStream();
            await _minioClient.GetObjectAsync(new GetObjectArgs()
                .WithBucket(bucketName)
                .WithObject(fileName)
                .WithCallbackStream(stream => stream.CopyTo(memoryStream)));

            memoryStream.Seek(0, SeekOrigin.Begin);
            return memoryStream;
        }

        public async Task<IEnumerable<string>> GetFilesByPrefixAsync(string prefix, string bucketName)
        {
            var fileNames = new List<string>();

            await foreach (var item in _minioClient.ListObjectsEnumAsync(
                new ListObjectsArgs()
                    .WithBucket(bucketName)
                    .WithPrefix(prefix)
                    .WithRecursive(true)))
            {
                if (!item.IsDir)
                {
                    fileNames.Add(item.Key);
                }
            }

            return fileNames;
        }

        public async Task<string> GeneratePresignedUrlAsync(string fileName, string bucketName, int expirySeconds = 3600)
        {
            var url = await _minioClient.PresignedGetObjectAsync(
                new PresignedGetObjectArgs()
                    .WithBucket(bucketName)
                    .WithObject(fileName)
                    .WithExpiry(expirySeconds)
            );
            return url;
        }

    }
}
