using Hospital.Models;
using Hospital.Repositories;
using Hospital.Services.Interfaces;

namespace Hospital.Services.Implementations
{
    public class MedicalRecordService : IMedicalRecordService
    {
        private readonly IRepository<MedicalRecord> _medicalRecordRepository;

        public MedicalRecordService(RepositoryFactory factory)
        {
            _medicalRecordRepository = factory.CreateRepository<MedicalRecord>();
        }

        public async Task AddAsync(MedicalRecord record)
        {
            await _medicalRecordRepository.AddAsync(record);
        }

        public async Task DeleteAsync(int id)
        {
            var record = await _medicalRecordRepository.GetAsync(id);
            if (record != null)
            {
                _medicalRecordRepository.Delete(record);
                await _medicalRecordRepository.SaveAsync();
            }
        }

        public async Task<MedicalRecord?> GetByIdAsync(int id)
        {
            return await _medicalRecordRepository.GetAsync(id);
        }

        public async Task<IEnumerable<MedicalRecord>> GetByPatientIdAsync(int patientId)
        {
            var records = await _medicalRecordRepository.GetAllAsync();
            return records.Where(p => p.PatientId == patientId);
        }

        public async Task UpdateAsync(MedicalRecord record)
        {
            _medicalRecordRepository.Update(record);
            await _medicalRecordRepository.SaveAsync();
        }
    }
}
