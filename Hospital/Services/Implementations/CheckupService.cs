using Hospital.Models;
using Hospital.Repositories;
using Hospital.Services.Interfaces;

namespace Hospital.Services.Implementations
{
    public class CheckupService : ICheckupService
    {

        private readonly IRepository<Checkup> _checkupRepository;

        public CheckupService(RepositoryFactory factory)
        {
            _checkupRepository = factory.CreateRepository<Checkup>();
        }
        public async Task AddAsync(Checkup checkup)
        {
            await _checkupRepository.AddAsync(checkup);
            await _checkupRepository.SaveAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var checkup = await _checkupRepository.GetAsync(id);
            if (checkup != null) {
                _checkupRepository.Delete(checkup);
                await _checkupRepository.SaveAsync();
            }
        }

        public async Task<Checkup?> GetByIdAsync(int id)
        {
            return await _checkupRepository.GetAsync(id);
        }

        public async Task<IEnumerable<Checkup>> GetByPatientIdAsync(int patientId)
        {
            var perscriptions = await _checkupRepository.GetAllAsync();
            return perscriptions.Where(p => p.PatientId == patientId);
        }

        public async Task UpdateAsync(Checkup checkup)
        {
            _checkupRepository.Update(checkup);
            await _checkupRepository.SaveAsync();
        }
    }
}
