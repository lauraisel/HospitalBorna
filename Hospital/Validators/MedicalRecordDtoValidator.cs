using FluentValidation;
using Hospital.DTOs;

namespace Hospital.Validators
{
    public class CreateMedicalRecordDtoValidator : AbstractValidator<CreateMedicalRecordDto>
    {
        public CreateMedicalRecordDtoValidator()
        {
            RuleFor(x => x.DiseaseName)
                .NotEmpty().WithMessage("Disease name is required.")
                .MaximumLength(100).WithMessage("Disease name must not exceed 100 characters.");

            RuleFor(x => x.StartDate)
                .LessThanOrEqualTo(DateTime.Now).WithMessage("Start date cannot be in the future.");

            RuleFor(x => x.PatientId)
                .GreaterThan(0).WithMessage("PatientId must be greater than zero.");

            RuleFor(x => x.EndDate)
                .GreaterThanOrEqualTo(x => x.StartDate)
                .When(x => x.EndDate.HasValue)
                .WithMessage("End date must be after start date.");
        }
    }

    public class UpdateMedicalRecordDtoValidator : AbstractValidator<UpdateMedicalRecordDto>
    {
        public UpdateMedicalRecordDtoValidator()
        {
            RuleFor(x => x.DiseaseName)
                .NotEmpty().WithMessage("Disease name is required.")
                .MaximumLength(100).WithMessage("Disease name must not exceed 100 characters.");

            RuleFor(x => x.StartDate)
                .LessThanOrEqualTo(DateTime.Now).WithMessage("Start date cannot be in the future.");

            RuleFor(x => x.EndDate)
                .GreaterThanOrEqualTo(x => x.StartDate)
                .When(x => x.EndDate.HasValue)
                .WithMessage("End date must be after start date.");

        }
    }
}
