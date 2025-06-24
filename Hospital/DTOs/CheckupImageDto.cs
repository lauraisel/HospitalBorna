namespace Hospital.DTOs
{
    public class CheckupImageDto
    {
        public int Id { get; set; }
        public string FileName { get; set; }
        public string ImageUrl { get; set; }
        public byte[]? FileContent { get; set; }
    }

    public class CreateCheckupImageDto
    {
        public IFormFile File { get; set; }
        public int CheckupId { get; set; }
    }
}
