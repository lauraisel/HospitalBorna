using CsvHelper.Configuration.Attributes;

namespace Hospital.DTOs
{
    public class MedicalRecordExportRow
    {
        [Name("Patient ID")]
        public int PatientId { get; set; }

        [Name("Full Name")]
        public string FullName { get; set; } = string.Empty;

        [Name("Sex")]
        public string Sex { get; set; } = string.Empty;

        [Name("Date of Birth")]
        public string DateOfBirth { get; set; } = string.Empty;

        [Name("Disease Name")]
        public string DiseaseName { get; set; } = string.Empty;

        [Name("Start Date")]
        public string StartDate { get; set; } = string.Empty;

        [Name("End Date")]
        public string EndDate { get; set; } = string.Empty;
    }
}
