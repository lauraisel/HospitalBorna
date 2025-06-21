using Hospital.Validators;
using FluentValidation;


namespace Hospital.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddAllValidators(this IServiceCollection services)
        {
            return services.AddValidatorsFromAssembly(typeof(CreatePatientDtoValidator).Assembly);
        }
    }
}
