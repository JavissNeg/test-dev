using FluentValidation;
using System.Reflection;

namespace TestDevBackJR.Infrastructure.Extensions;

public static class ValidationExtensions
{
    public static IServiceCollection AddApplicationValidators(this IServiceCollection services)
    {
        var assembly = typeof(TestDevBackJR.Application.Validators.LoginValidator).Assembly;

        var validatorTypes = assembly.GetTypes()
            .Where(t => t.IsClass && !t.IsAbstract && t.BaseType?.IsGenericType == true)
            .Where(t => t.BaseType?.GetGenericTypeDefinition() == typeof(AbstractValidator<>))
            .ToList();

        foreach (var validatorType in validatorTypes)
        {
            var interfaceType = validatorType.BaseType?.GetGenericArguments()[0];
            if (interfaceType != null)
            {
                var genericValidatorInterface = typeof(IValidator<>).MakeGenericType(interfaceType);
                services.AddScoped(genericValidatorInterface, validatorType);
            }
        }

        return services;
    }
}

