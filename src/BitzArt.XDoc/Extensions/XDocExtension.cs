using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;

namespace BitzArt.XDoc;

/// <summary>
/// Extension methods for <see cref="XDoc"/>.
/// </summary>
public static class XDocExtension
{
    /// <summary>
    /// Registers <see cref="XDoc"/> as a singleton service.
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    [PublicAPI]
    public static IServiceCollection AddXDoc(this IServiceCollection services)
    {
        return services.AddSingleton<XDoc>();
    }
}