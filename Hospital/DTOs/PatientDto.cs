using Hospital.Models;

namespace Hospital.DTOs
{
    public class PatientDto
    {
        public int Id { get; set; }
        public string PersonalId { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public DateTime DateOfBirth { get; set; }
        public Sex Sex { get; set; }
    }

    public class CreatePatientDto
    {
        public string PersonalId { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public DateTime DateOfBirth { get; set; }
        public Sex Sex { get; set; }
    }
}
