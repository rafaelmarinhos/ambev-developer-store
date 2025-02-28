using Microsoft.Extensions.DependencyInjection;

namespace Ambev.DeveloperEvaluation.IoC.Extensions;

public static class CorsConfigExtension
{
    public static void AddCorsConfiguration(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddCors(options =>
        {
            options.AddDefaultPolicy(
                corsPolicyBuilder =>
                {
                    corsPolicyBuilder
                        .AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader();
                });
        });
    }
}
