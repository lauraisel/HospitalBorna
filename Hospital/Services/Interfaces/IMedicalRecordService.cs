using Hospital.Models;
using Microsoft.AspNetCore.Mvc;

namespace Hospital.Services.Interfaces
{
    public interface IMedicalRecordService
    {
        Task<IEnumerable<MedicalRecord>> GetByPatientIdAsync(int patientId);
        Task<MedicalRecord?> GetByIdAsync(int id);
        Task AddAsync(MedicalRecord record);
        Task UpdateAsync(MedicalRecord record);
        Task DeleteAsync(int id);
        Task<FileStreamResult> GetMedicalRecordsCsvAsync(int patientId);
    }
}
