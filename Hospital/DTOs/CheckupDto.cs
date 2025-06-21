using Hospital.Models;

namespace Hospital.DTOs
{
    public class CheckupDto
    {
        public int Id { get; set; }
        public DateTime CheckupTime { get; set; }
        public CheckupProcedure Procedure { get; set; }
        public string? ImagePath { get; set; }
        public int PatientId { get; set; }
    }

    public class CreateCheckupDto
    {
        public DateTime CheckupTime { get; set; }
        public CheckupProcedure Procedure { get; set; }
        public string? ImagePath { get; set; }
        public int PatientId { get; set; }
    }
}
