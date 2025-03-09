using Microsoft.Extensions.DependencyInjection;

namespace BitzArt.XDoc;

/// <summary>
/// Extension methods for <see cref="IXDoc"/>.
/// </summary>
public static class XDocExtension
{
    /// <summary>
    /// Registers <see cref="IXDoc"/> as a singleton service.
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddXDoc(this IServiceCollection services)
    {
        return services.AddSingleton<IXDoc, XDoc>();
    }
}