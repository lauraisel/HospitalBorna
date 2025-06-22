using AutoMapper;
using Hospital.DTOs;
using Hospital.Models;
using Hospital.Repositories;
using Hospital.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Hospital.Services.Implementations
{
    public class PrescriptionService : IPrescriptionService
    {
        private readonly RepositoryFactory _factory;
        private IRepository<Prescription>? _prescriptionRepository;
        private readonly IMapper _mapper;

        private IRepository<Prescription> PrescriptionRepository =>
        _prescriptionRepository ??= _factory.CreateRepository<Prescription>();

        public PrescriptionService(RepositoryFactory factory, IMapper mapper)
        {
            _factory = factory;
            _mapper = mapper;
        }

        public async Task<IEnumerable<PrescriptionDto>> GetAllAsync()
        {
            var prescriptions = await PrescriptionRepository.GetAll().ToListAsync();
            return _mapper.Map<IEnumerable<PrescriptionDto>>(prescriptions);
        }

        public async Task<PrescriptionDto?> GetByIdAsync(int id)
        {
            var prescription = await PrescriptionRepository.GetAsync(id);
            return prescription == null ? null : _mapper.Map<PrescriptionDto>(prescription);
        }

        public async Task<PrescriptionDto> CreatePrescriptionAsync(CreatePrescriptionDto createDto)
        {
            var prescription = _mapper.Map<Prescription>(createDto);
            await PrescriptionRepository.AddAsync(prescription);
            await PrescriptionRepository.SaveAsync();
            return _mapper.Map<PrescriptionDto>(prescription);
        }

        public async Task<bool> UpdatePrescriptionAsync(int id, UpdatePrescriptionDto updateDto)
        {
            var existing = await PrescriptionRepository.GetAsync(id);
            if (existing == null)
                return false;

            _mapper.Map(updateDto, existing);
            PrescriptionRepository.Update(existing);
            await PrescriptionRepository.SaveAsync();
            return true;
        }

        public async Task<bool> DeletePrescriptionAsync(int id)
        {
            var prescription = await PrescriptionRepository.GetAsync(id);
            if (prescription == null)
                return false;

            PrescriptionRepository.Delete(prescription);
            await PrescriptionRepository.SaveAsync();
            return true;
        }

        public async Task<IEnumerable<PrescriptionDto>> GetByCheckupIdAsync(int checkupId)
        {
            var prescriptions = await PrescriptionRepository.GetAll()
                .Where(c => c.CheckupId == checkupId)
                .ToListAsync();

            return _mapper.Map<IEnumerable<PrescriptionDto>>(prescriptions);
        }
    }
}
