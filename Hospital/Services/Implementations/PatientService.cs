using Hospital.Models;
using Hospital.Repositories;
using Hospital.Services.Interfaces;

namespace Hospital.Services.Implementations
{
    public class PatientService : IPatientService
    {
        private readonly IRepository<Patient> _patientRepository;

        public PatientService(RepositoryFactory factory)
        {
            _patientRepository = factory.CreateRepository<Patient>();
        }

        public async Task<IEnumerable<Patient>> GetAllAsync()
        {
            return await _patientRepository.GetAllAsync();
        }

        public async Task<Patient?> GetByIdAsync(int id)
        {
            return await _patientRepository.GetAsync(id);
        }

        public async Task AddAsync(Patient patient)
        {
            await _patientRepository.AddAsync(patient);
            await _patientRepository.SaveAsync();
        }

        public async Task UpdateAsync(Patient patient)
        {
            _patientRepository.Update(patient);
            await _patientRepository.SaveAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var patient = await _patientRepository.GetAsync(id);
            if (patient != null)
            {
                _patientRepository.Delete(patient);
                await _patientRepository.SaveAsync();
            }
        }
    }
}
