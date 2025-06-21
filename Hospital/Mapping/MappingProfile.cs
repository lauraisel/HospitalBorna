using AutoMapper;
using Hospital.DTOs;
using Hospital.Models;

namespace Hospital.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Patient, PatientDto>();
            CreateMap<CreatePatientDto, Patient>();

            CreateMap<Checkup, CheckupDto>();
            CreateMap<CreateCheckupDto, Checkup>();

            CreateMap<MedicalRecord, MedicalRecordDto>();
            CreateMap<CreateMedicalRecordDto, MedicalRecord>();

            CreateMap<Prescription, PrescriptionDto>();
            CreateMap<CreatePrescriptionDto, Prescription>();
        }
    }
}
