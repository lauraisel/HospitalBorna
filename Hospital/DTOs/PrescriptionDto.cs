namespace Hospital.DTOs
{
    public class PrescriptionDto
    {
        public int Id { get; set; }
        public string Medication { get; set; }
        public string Dosage { get; set; }
        public int CheckupId { get; set; }
    }

    public class CreatePrescriptionDto
    {
        public string Medication { get; set; }
        public string Dosage { get; set; }
        public int CheckupId { get; set; }
    }

    public class UpdatePrescriptionDto
    {
        public string Medication { get; set; }
        public string Dosage { get; set; }
    }
}
