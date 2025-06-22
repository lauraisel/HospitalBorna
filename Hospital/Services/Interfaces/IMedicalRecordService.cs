using Hospital.DTOs;
using Hospital.Models;
using Microsoft.AspNetCore.Mvc;

namespace Hospital.Services.Interfaces
{
    public interface IMedicalRecordService
    {
        Task<IEnumerable<MedicalRecordDto>> GetAllAsync();
        Task<MedicalRecordDto?> GetByIdAsync(int id);
        Task<MedicalRecordDto> CreateMedicalRecordAsync(CreateMedicalRecordDto createDto);
        Task<bool> UpdateMedicalRecordAsync(int id, UpdateMedicalRecordDto updateDto);
        Task<bool> DeleteMedicalRecordAsync(int id);
        Task<IEnumerable<MedicalRecordDto>> GetByPatientIdAsync(int patientId);
        Task<FileStreamResult> GetMedicalRecordsCsvAsync(int patientId);
    }
}
