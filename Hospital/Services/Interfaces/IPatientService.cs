using Hospital.DTOs;
using Hospital.Models;

namespace Hospital.Services.Interfaces
{
    public interface IPatientService
    {
        Task<IEnumerable<PatientDto>> GetAllAsync();
        Task<PatientDto?> GetByIdAsync(int id);
        Task<PatientDto> CreatePatientAsync(CreatePatientDto createDto);
        Task<bool> UpdatePatientAsync(int id, UpdatePatientDto updateDto);
        Task<bool> DeletePatientAsync(int id);
    }
}
