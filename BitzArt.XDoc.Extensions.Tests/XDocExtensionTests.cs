using Microsoft.Extensions.DependencyInjection;

namespace BitzArt.XDoc.Extensions.Tests;

public class XDocExtensionTests
{
    [Fact]
    public void AddXDoc_RegistersIXDocAsSingleton()
    {
        // Arrange
        var services = new ServiceCollection();
            
        // Act
        services.AddXDoc();
            
        // Assert
        var serviceDescriptor = services.FirstOrDefault(s => s.ServiceType == typeof(IXDoc));
            
        Assert.NotNull(serviceDescriptor);
        Assert.Equal(ServiceLifetime.Singleton, serviceDescriptor.Lifetime);
        Assert.Equal(typeof(XDoc), serviceDescriptor.ImplementationType);
    }
}