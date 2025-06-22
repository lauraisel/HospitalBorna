using Hospital.DTOs;
using Hospital.Models;

namespace Hospital.Services.Interfaces
{
    public interface ICheckupService
    {
        Task<IEnumerable<CheckupDto>> GetAllAsync();
        Task<CheckupDto?> GetByIdAsync(int id);
        Task<CheckupDto> CreateCheckupAsync(CreateCheckupDto createDto);
        Task<bool> UpdateCheckupAsync(int id, UpdateCheckupDto updateDto);
        Task<bool> DeleteCheckupAsync(int id);
        Task<IEnumerable<CheckupDto>> GetByPatientIdAsync(int patientId);
    }
}
