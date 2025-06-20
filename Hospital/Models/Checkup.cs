namespace Hospital.Models
{
    public class Checkup
    {
        public int Id { get; set; }
        public DateTime CheckupTime { get; set; }
        public CheckupProcedure Procedure { get; set; }
        public string? ImagePath { get; set; }

        public int PatientId { get; set; }
        public virtual Patient Patient { get; set; }

        public virtual ICollection<Prescription> Prescriptions { get; set; } = new List<Prescription>();
    }

}
