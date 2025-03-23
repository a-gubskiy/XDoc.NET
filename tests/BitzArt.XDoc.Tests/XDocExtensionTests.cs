using Microsoft.Extensions.DependencyInjection;

namespace BitzArt.XDoc.Tests;

public class XDocExtensionTests
{
    [Fact]
    public void AddXDoc_RegistersXDocAsSingleton()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        services.AddXDoc();

        var serviceDescriptor = services.First();
        
        // Assert
        Assert.Single(services);
        Assert.Equal(typeof(XDoc), serviceDescriptor.ServiceType);
        Assert.Equal(ServiceLifetime.Singleton, serviceDescriptor.Lifetime);
    }

    [Fact]
    public void AddXDoc_ReturnsServiceCollection()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        var result = services.AddXDoc();

        // Assert
        Assert.Same(services, result);
    }

    [Fact]
    public void AddXDoc_CalledMultipleTimes_RegistersXDocOnce()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        services.AddXDoc();
        services.AddXDoc();

        // Assert
        Assert.Single(services, s => s.ServiceType == typeof(XDoc) && s.Lifetime == ServiceLifetime.Singleton);
    }

    [Fact]
    public void AddXDoc_RegistersService_GetRequiredServiceShouldReturService()
    {
        // Arrange
        var services = new ServiceCollection();

        services.AddXDoc();
        services.AddXDoc();

        // Act
        var serviceProvider = services.BuildServiceProvider();
        var xDoc = serviceProvider.GetRequiredService<XDoc>();

        // Assert
        Assert.NotNull(xDoc);
        Assert.Equal(typeof(XDoc), xDoc.GetType());
    }
}