using FluentValidation;
using Hospital.DTOs;

namespace Hospital.Validators
{
    public class CreateCheckupImageDtoValidator : AbstractValidator<CreateCheckupImageDto>
    {
        private readonly string[] _allowedContentTypes = new[]
        {
            "image/jpeg",
            "image/png",
            "image/gif",
            "image/bmp",
            "image/webp"
        };

        public CreateCheckupImageDtoValidator()
        {
            RuleFor(x => x.File)
                .NotNull().WithMessage("File is required.")
                .Must(BeAValidImage).WithMessage("File must be a valid image type (jpeg, png, gif, bmp, webp).")
                .Must(f => f.Length > 0).WithMessage("File cannot be empty.")
                .Must(f => f.Length <= 5 * 1024 * 1024)
                    .WithMessage("File size must be less than 5 MB.");

            RuleFor(x => x.CheckupId)
                .GreaterThan(0).WithMessage("Valid CheckupId is required.");
        }

        private bool BeAValidImage(IFormFile file)
        {
            if (file == null) return false;
            return _allowedContentTypes.Contains(file.ContentType);
        }
    }
}
