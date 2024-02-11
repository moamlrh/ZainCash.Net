using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ZainCash.Net.Services;
using ZainCash.Net.Utils;

namespace ZainCash.Net.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddZainCashService(this IServiceCollection services, bool isDevelopment = false)
    {
        if (isDevelopment)
            services.AddScoped<IZainCashService, TestZainCashService>();
        else
            services.AddScoped<IZainCashService, ZainCashService>();
        return services;
    }
    public static IServiceCollection AddZainCashConfig(this IServiceCollection services, IConfiguration configuration)
        => AddZainCashConfig(services, "ZainCashAPIConfig", configuration);

    public static IServiceCollection AddZainCashConfig(this IServiceCollection services, string sectionName, IConfiguration configuration)
    {
        var config = configuration.GetSection(sectionName).Get<ZainCashAPIConfig>();
        if (config == null)
            throw new ArgumentException($"Add {sectionName} to appsettings.json.");
        return AddZainCashConfig(services, config);
    }

    public static IServiceCollection AddZainCashConfig(this IServiceCollection services, ZainCashAPIConfig config)
    {
        services.AddSingleton(config);
        return services;
    }
}
