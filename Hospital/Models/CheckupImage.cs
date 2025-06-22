using System.ComponentModel.DataAnnotations;

namespace Hospital.Models
{
    public class CheckupImage
    {
        public int Id { get; set; }
        public string FileName { get; set; }

        public int CheckupId { get; set; }

        public virtual Checkup Checkup { get; set; }
    }
}
