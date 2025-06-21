using Microsoft.Extensions.DependencyInjection;

namespace OllamaAI;

public static class DependencyInjection
{
    public static IServiceCollection AddOllamaHttpClient(this IServiceCollection services,
        string baseUrl,
        string modelId)
    {
        services.AddScoped<IOllamaHttpClient>(provider => new OllamaHttpClient(baseUrl, modelId));
        return services;
    }
}
