using Microsoft.Extensions.DependencyInjection;

namespace BitzArt.XDoc.Extensions;

public static class XDocExtension
{
    public static IServiceCollection AddXDoc(this IServiceCollection services)
    {
        return services.AddSingleton<IXDoc, XDoc>();
    }
}