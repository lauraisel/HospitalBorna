using FluentValidation;
using Hospital.DTOs;

namespace Hospital.Validators
{
    public class CreateCheckupDtoValidator : AbstractValidator<CreateCheckupDto>
    {
        public CreateCheckupDtoValidator()
        {
            RuleFor(x => x.CheckupTime)
                .NotEmpty().WithMessage("Checkup time is required.")
                .LessThanOrEqualTo(DateTime.Now).WithMessage("Checkup time cannot be in the future.");

            RuleFor(x => x.Procedure)
                .IsInEnum().WithMessage("Procedure must be a valid value.");

            RuleFor(x => x.PatientId)
                .GreaterThan(0).WithMessage("PatientId must be greater than zero.");

            RuleFor(x => x.ImagePath)
                .MaximumLength(255).WithMessage("Image path must not exceed 255 characters.")
                .When(x => !string.IsNullOrEmpty(x.ImagePath));
        }
    }

    public class UpdateCheckupDtoValidator : AbstractValidator<UpdateCheckupDto>
    {
        public UpdateCheckupDtoValidator()
        {
            RuleFor(x => x.CheckupTime)
                .NotEmpty().WithMessage("Checkup time is required.")
                .LessThanOrEqualTo(DateTime.Now).WithMessage("Checkup time cannot be in the future.");

            RuleFor(x => x.Procedure)
                .IsInEnum().WithMessage("Procedure must be a valid value.");

            RuleFor(x => x.ImagePath)
                .MaximumLength(255).WithMessage("Image path must not exceed 255 characters.")
                .When(x => !string.IsNullOrEmpty(x.ImagePath));

        }
    }
}
