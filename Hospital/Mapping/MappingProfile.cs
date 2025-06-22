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
            CreateMap<UpdatePatientDto, Patient>();

            CreateMap<Checkup, CheckupDto>();
            CreateMap<CreateCheckupDto, Checkup>();
            CreateMap<UpdateCheckupDto, Checkup>();

            CreateMap<MedicalRecord, MedicalRecordDto>();
            CreateMap<CreateMedicalRecordDto, MedicalRecord>();
            CreateMap<UpdateMedicalRecordDto, MedicalRecord>();

            CreateMap<Prescription, PrescriptionDto>();
            CreateMap<CreatePrescriptionDto, Prescription>();
            CreateMap<UpdatePrescriptionDto, Prescription>();
        }
    }
}
