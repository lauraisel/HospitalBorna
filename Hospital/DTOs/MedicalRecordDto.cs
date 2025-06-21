namespace Hospital.DTOs
{
    public class MedicalRecordDto
    {
        public int Id { get; set; }
        public string DiseaseName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int PatientId { get; set; }
    }

    public class CreateMedicalRecordDto
    {
        public string DiseaseName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int PatientId { get; set; }
    }

    public class UpdateMedicalRecordDto
    {
        public string DiseaseName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
