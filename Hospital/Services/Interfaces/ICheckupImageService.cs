using Hospital.DTOs;

namespace Hospital.Services.Interfaces
{
    public interface ICheckupImageService
    {
        Task<CheckupImageDto> UploadImageAsync(CreateCheckupImageDto dto);
        Task<string> GetImageUrlAsync(int imageId);
        Task<List<CheckupImageDto>> GetImagesByCheckupIdAsync(int checkupId);
    }
}
