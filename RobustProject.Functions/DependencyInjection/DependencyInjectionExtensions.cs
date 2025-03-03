using Azure.Core;
using Azure.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace RobustProject.Functions.DependencyInjection;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddFunctionDependencies(this IServiceCollection services)
    {
        return services
            .AddSingleton<TokenCredential>(_ => new DefaultAzureCredential());
    }
}