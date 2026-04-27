using System.Text.Json.Serialization;

namespace TestDevBackJR.Infrastructure.Extensions;

public static class ControllerExtensions
{
    public static IServiceCollection AddControllerConfiguration(this IServiceCollection services)
    {
        services.AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
            });

        return services;
    }
}

