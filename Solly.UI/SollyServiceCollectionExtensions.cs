using Microsoft.Extensions.DependencyInjection;
using Solly.UI.Core;
using Solly.UI.Core.Interop;

namespace Solly.UI;

public static class SollyServiceCollectionExtensions
{
    public static IServiceCollection AddSollyUI(this IServiceCollection services,
        Action<SollyOptions>? configure = null)
    {
        services.Configure(configure ?? (_ => { }));
        services.AddScoped<SollyInterop>();
        services.AddScoped<SollyThemeService>();
        services.AddScoped<SToastService>();
        return services;
    }
}