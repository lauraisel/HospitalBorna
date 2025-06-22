using AutoMapper;
using Hospital.DTOs;
using Hospital.Models;
using Hospital.Repositories;
using Hospital.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Hospital.Services.Implementations
{
    public class CheckupService : ICheckupService
    {
        private readonly RepositoryFactory _factory;
        private IRepository<Checkup>? _checkupRepository;
        private readonly IMapper _mapper;

        private IRepository<Checkup> CheckupRepository =>
        _checkupRepository ??= _factory.CreateRepository<Checkup>();

        public CheckupService(RepositoryFactory factory, IMapper mapper)
        {
            _factory = factory;
            _mapper = mapper;
        }
        public async Task<IEnumerable<CheckupDto>> GetAllAsync()
        {
            var checkups = await CheckupRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<CheckupDto>>(checkups);
        }

        public async Task<CheckupDto?> GetByIdAsync(int id)
        {
            var checkup = await CheckupRepository.GetAsync(id);
            return checkup == null ? null : _mapper.Map<CheckupDto>(checkup);
        }

        public async Task<CheckupDto> CreateCheckupAsync(CreateCheckupDto createDto)
        {
            var checkup = _mapper.Map<Checkup>(createDto);
            await CheckupRepository.AddAsync(checkup);
            await CheckupRepository.SaveAsync();
            return _mapper.Map<CheckupDto>(checkup);
        }

        public async Task<bool> UpdateCheckupAsync(int id, UpdateCheckupDto updateDto)
        {
            var existing = await CheckupRepository.GetAsync(id);
            if (existing == null)
                return false;

            _mapper.Map(updateDto, existing);
            CheckupRepository.Update(existing);
            await CheckupRepository.SaveAsync();
            return true;
        }

        public async Task<bool> DeleteCheckupAsync(int id)
        {
            var checkup = await CheckupRepository.GetAsync(id);
            if (checkup == null)
                return false;

            CheckupRepository.Delete(checkup);
            await CheckupRepository.SaveAsync();
            return true;
        }

        public async Task<IEnumerable<CheckupDto>> GetByPatientIdAsync(int patientId)
        {
            var checkups = await CheckupRepository
                .GetAll()
                .Where(c => c.PatientId == patientId)
                .ToListAsync();

            return _mapper.Map<IEnumerable<CheckupDto>>(checkups);
        }
    }
}
