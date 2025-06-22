using Hospital.DTOs;
using Hospital.Models;

namespace Hospital.Services.Interfaces
{
    public interface IPrescriptionService
    {
        Task<IEnumerable<PrescriptionDto>> GetAllAsync();
        Task<PrescriptionDto?> GetByIdAsync(int id);
        Task<PrescriptionDto> CreatePrescriptionAsync(CreatePrescriptionDto createDto);
        Task<bool> UpdatePrescriptionAsync(int id, UpdatePrescriptionDto updateDto);
        Task<bool> DeletePrescriptionAsync(int id);
        Task<IEnumerable<PrescriptionDto>> GetByCheckupIdAsync(int checkupId);
    }
}
