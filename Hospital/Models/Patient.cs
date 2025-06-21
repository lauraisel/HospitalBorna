namespace Hospital.Models
{
    public class Patient
    {
        public int Id { get; set; }
        public string PersonalId { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public DateTime DateOfBirth { get; set; }
        public Sex Sex { get; set; }

        public virtual ICollection<MedicalRecord> MedicalRecords { get; set; } = new List<MedicalRecord>();
        public virtual ICollection<Checkup> Checkups { get; set; } = new List<Checkup>();
    }

}
