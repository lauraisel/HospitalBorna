using AutoMapper;
using Hospital.DTOs;
using Hospital.Models;
using Hospital.Repositories;
using Hospital.Services.Interfaces;

namespace Hospital.Services.Implementations
{
    public class PatientService : IPatientService
    {
        private readonly RepositoryFactory _factory;
        private IRepository<Patient>? _patientRepository;
        private readonly IMapper _mapper;

        public PatientService(RepositoryFactory factory, IMapper mapper)
        {
            _factory = factory;
            _mapper = mapper;
        }

        private IRepository<Patient> PatientRepository =>
            _patientRepository ??= _factory.CreateRepository<Patient>();

        public async Task<IEnumerable<PatientDto>> GetAllAsync()
        {
            var patients = await PatientRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<PatientDto>>(patients);
        }

        public async Task<PatientDto?> GetByIdAsync(int id)
        {
            var patient = await PatientRepository.GetAsync(id);
            return patient == null ? null : _mapper.Map<PatientDto>(patient);
        }

        public async Task<PatientDto> CreatePatientAsync(CreatePatientDto createDto)
        {
            var patient = _mapper.Map<Patient>(createDto);
            await PatientRepository.AddAsync(patient);
            await PatientRepository.SaveAsync();
            return _mapper.Map<PatientDto>(patient);
        }

        public async Task<bool> UpdatePatientAsync(int id, UpdatePatientDto updateDto)
        {
            var existingPatient = await PatientRepository.GetAsync(id);
            if (existingPatient == null)
                return false;

            _mapper.Map(updateDto, existingPatient);

            PatientRepository.Update(existingPatient);
            await PatientRepository.SaveAsync();
            return true;
        }

        public async Task<bool> DeletePatientAsync(int id)
        {
            var patient = await PatientRepository.GetAsync(id);
            if (patient == null)
                return false;

            PatientRepository.Delete(patient);
            await PatientRepository.SaveAsync();
            return true;
        }
    }
}
