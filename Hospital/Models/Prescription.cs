namespace Hospital.Models
{
    public class Prescription
    {
        public int Id { get; set; }
        public string Medication { get; set; }
        public string Dosage { get; set; }

        public int CheckupId { get; set; }
        public virtual Checkup Checkup { get; set; }
    }

}
