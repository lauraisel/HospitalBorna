namespace Hospital.Services.Interfaces
{
    public interface IMinioStorageService
    {
        Task<string> UploadFileAsync(Stream stream, string fileName, string contentType, string bucketName);
        Task<Stream?> GetFileAsync(string fileName, string bucketName);
        Task<IEnumerable<string>> GetFilesByPrefixAsync(string prefix, string bucketName);
        Task<string> GeneratePresignedUrlAsync(string fileName, string bucketName, int expirySeconds = 3600);
    }
}
