using Hospital.Models;

namespace Hospital.Services.Interfaces
{
    public interface ICheckupService
    {
        Task<IEnumerable<Checkup>> GetByPatientIdAsync(int patientId);
        Task<Checkup?> GetByIdAsync(int id);
        Task AddAsync(Checkup checkup);
        Task UpdateAsync(Checkup checkup);
        Task DeleteAsync(int id);
    }
}
