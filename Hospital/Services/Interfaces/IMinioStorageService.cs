using Minio.DataModel.Args;

namespace Hospital.Services.Interfaces
{
    public interface IMinioStorageService
    {
        Task<string> UploadFileAsync(Stream stream, string fileName, string contentType, string bucketName);
        Task<Stream?> GetFileAsync(string fileName, string bucketName);
        Task<IEnumerable<string>> GetFilesByPrefixAsync(string prefix, string bucketName);
        Task<bool> BucketExistsAsync(BucketExistsArgs args);
        Task MakeBucketAsync(MakeBucketArgs args);
        Task<List<string>> ListBucketsAsync();

    }
}
