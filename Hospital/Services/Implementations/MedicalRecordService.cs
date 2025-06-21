using CsvHelper;
using Hospital.DTOs;
using Hospital.Models;
using Hospital.Repositories;
using Hospital.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Formats.Asn1;
using System.Globalization;

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

        public async Task<FileStreamResult> GetMedicalRecordsCsvAsync(int patientId)
        {
            var records = await _medicalRecordRepository.GetAll()
                .Where(r => r.PatientId == patientId)
                .Include(r => r.Patient)
                .Select(r => new MedicalRecordExportRow
                {
                    PatientId = r.PatientId,
                    FullName = r.Patient.Name + " " + r.Patient.Surname,
                    Sex = r.Patient.Sex.ToString(),
                    DateOfBirth = r.Patient.DateOfBirth.ToString("yyyy-MM-dd"),
                    DiseaseName = r.DiseaseName,
                    StartDate = r.StartDate.ToString("yyyy-MM-dd"),
                    EndDate = r.EndDate != null ? r.EndDate.Value.ToString("yyyy-MM-dd") : ""
                })
                .ToListAsync();

            var memoryStream = new MemoryStream();
            using (var writer = new StreamWriter(memoryStream, leaveOpen: true))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                await csv.WriteRecordsAsync(records);
                await writer.FlushAsync();
            }

            memoryStream.Position = 0;
            var fileName = $"medical_records_patient_{patientId}.csv";
            return new FileStreamResult(memoryStream, "text/csv")
            {
                FileDownloadName = fileName
            };
        }

        public async Task UpdateAsync(MedicalRecord record)
        {
            _medicalRecordRepository.Update(record);
            await _medicalRecordRepository.SaveAsync();
        }
    }
}
