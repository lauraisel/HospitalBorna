using Hospital.Models;
using Hospital.Repositories;
using Hospital.Services.Interfaces;

namespace Hospital.Services.Implementations
{
    public class PrescriptionService : IPrescriptionService
    {
        private readonly IRepository<Prescription> _prescriptionRepository;

        public PrescriptionService(RepositoryFactory factory)
        {
            _prescriptionRepository = factory.CreateRepository<Prescription>();
        }

        public async Task AddAsync(Prescription prescription)
        {
            await _prescriptionRepository.AddAsync(prescription);
            await _prescriptionRepository.SaveAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var prescription = await _prescriptionRepository.GetAsync(id);
            if (prescription != null)
            {
                _prescriptionRepository.Delete(prescription);
                await _prescriptionRepository.SaveAsync();
            }
        }

        public async Task<IEnumerable<Prescription>> GetByCheckupIdAsync(int checkupId)
        {
            var perscriptions = await _prescriptionRepository.GetAllAsync();
            return perscriptions.Where(c => c.CheckupId == checkupId);
        }

        public async Task<Prescription?> GetByIdAsync(int id)
        {
            return await _prescriptionRepository.GetAsync(id);
        }

        public async Task UpdateAsync(Prescription prescription)
        {
            _prescriptionRepository.Update(prescription);
            await _prescriptionRepository.SaveAsync();
        }
    }
}
