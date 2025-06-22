using AutoMapper;
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
        private readonly RepositoryFactory _factory;
        private IRepository<MedicalRecord>? _medicalRecordRepository;
        private readonly IMapper _mapper;

        private IRepository<MedicalRecord> MedicalRecordRepository =>
        _medicalRecordRepository ??= _factory.CreateRepository<MedicalRecord>();

        public MedicalRecordService(RepositoryFactory factory, IMapper mapper)
        {
            _factory = factory;
            _mapper = mapper;
        }

        public async Task<IEnumerable<MedicalRecordDto>> GetAllAsync()
        {
            var records = await MedicalRecordRepository.GetAll().ToListAsync();
            return _mapper.Map<IEnumerable<MedicalRecordDto>>(records);
        }

        public async Task<MedicalRecordDto?> GetByIdAsync(int id)
        {
            var record = await MedicalRecordRepository.GetAsync(id);
            return record == null ? null : _mapper.Map<MedicalRecordDto>(record);
        }

        public async Task<MedicalRecordDto> CreateMedicalRecordAsync(CreateMedicalRecordDto createDto)
        {
            var record = _mapper.Map<MedicalRecord>(createDto);
            await MedicalRecordRepository.AddAsync(record);
            await MedicalRecordRepository.SaveAsync();
            return _mapper.Map<MedicalRecordDto>(record);
        }

        public async Task<bool> UpdateMedicalRecordAsync(int id, UpdateMedicalRecordDto updateDto)
        {
            var existing = await MedicalRecordRepository.GetAsync(id);
            if (existing == null)
                return false;

            _mapper.Map(updateDto, existing);
            MedicalRecordRepository.Update(existing);
            await MedicalRecordRepository.SaveAsync();
            return true;
        }

        public async Task<bool> DeleteMedicalRecordAsync(int id)
        {
            var record = await MedicalRecordRepository.GetAsync(id);
            if (record == null)
                return false;

            MedicalRecordRepository.Delete(record);
            await MedicalRecordRepository.SaveAsync();
            return true;
        }

        public async Task<IEnumerable<MedicalRecordDto>> GetByPatientIdAsync(int patientId)
        {
            var records = await MedicalRecordRepository.GetAll()
                .Where(r => r.PatientId == patientId)
                .ToListAsync();

            return _mapper.Map<IEnumerable<MedicalRecordDto>>(records);
        }

        public async Task<FileStreamResult> GetMedicalRecordsCsvAsync(int patientId)
        {
            var records = await MedicalRecordRepository.GetAll()
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
    }
}
