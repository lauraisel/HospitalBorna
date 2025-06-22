using FluentValidation;
using Hospital.DTOs;

namespace Hospital.Validators
{
    public class CreatePrescriptionDtoValidator : AbstractValidator<CreatePrescriptionDto>
    {
        public CreatePrescriptionDtoValidator()
        {
            RuleFor(x => x.Medication)
                .NotEmpty().WithMessage("Medication is required.")
                .MaximumLength(100).WithMessage("Medication must not exceed 100 characters.");

            RuleFor(x => x.Dosage)
                .NotEmpty().WithMessage("Dosage is required.")
                .MaximumLength(50).WithMessage("Dosage must not exceed 50 characters.");

            RuleFor(x => x.CheckupId)
                .GreaterThan(0).WithMessage("CheckupId must be greater than zero.");
        }
    }

    public class UpdatePrescriptionDtoValidator : AbstractValidator<UpdatePrescriptionDto>
    {
        public UpdatePrescriptionDtoValidator()
        {
            RuleFor(x => x.Medication)
                .NotEmpty().WithMessage("Medication is required.")
                .MaximumLength(100).WithMessage("Medication must not exceed 100 characters.");

            RuleFor(x => x.Dosage)
                .NotEmpty().WithMessage("Dosage is required.")
                .MaximumLength(50).WithMessage("Dosage must not exceed 50 characters.");
        }
    }
}
