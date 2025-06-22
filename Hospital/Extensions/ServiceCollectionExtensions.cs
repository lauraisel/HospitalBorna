using Hospital.Validators;
using FluentValidation;
using Hospital.Infrastructure;


namespace Hospital.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddAllValidators(this IServiceCollection services)
        {
            return services.AddValidatorsFromAssembly(typeof(CreatePatientDtoValidator).Assembly);
        }

        public static IServiceCollection AddLazyResolution(this IServiceCollection services)
        {
            services.AddTransient(typeof(Lazy<>), typeof(LazyResolver<>));
            return services;
        }
    }
}
