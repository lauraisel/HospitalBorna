using Hospital.Services.Interfaces;
using Minio;
using Minio.DataModel.Args;

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
            using var ms = new MemoryStream();
            await stream.CopyToAsync(ms);
            ms.Position = 0;

            await _minioClient.PutObjectAsync(new PutObjectArgs()
                .WithBucket(bucketName)
                .WithObject(fileName)
                .WithStreamData(ms)
                .WithObjectSize(ms.Length)
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

        public async Task<bool> BucketExistsAsync(BucketExistsArgs args)
        {
            return await _minioClient.BucketExistsAsync(args);
        }

        public async Task MakeBucketAsync(MakeBucketArgs args)
        {
            await _minioClient.MakeBucketAsync(args);
        }

        public async Task<List<string>> ListBucketsAsync()
        {
            var bucketList = await _minioClient.ListBucketsAsync();

            if (bucketList?.Buckets == null)
                return new List<string>();

            return bucketList.Buckets.Select(b => b.Name).ToList();
        }
    }
}
