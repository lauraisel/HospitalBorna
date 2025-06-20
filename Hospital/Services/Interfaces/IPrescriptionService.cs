using Hospital.Models;

namespace Hospital.Services.Interfaces
{
    public interface IPrescriptionService
    {
        Task<IEnumerable<Prescription>> GetByCheckupIdAsync(int checkupId);
        Task<Prescription?> GetByIdAsync(int id);
        Task AddAsync(Prescription prescription);
        Task UpdateAsync(Prescription prescription);
        Task DeleteAsync(int id);
    }
}
