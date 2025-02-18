using TestAssembly;
using TestAssembly.A;

namespace Xdoc.Base.Tests;

public class FastDocumentationStoreTests
{
    [Fact]
    public async Task Test1()
    {
        var x = new ClassA();
        
        var assemblies = AppDomain.CurrentDomain.GetAssemblies();
        var assembly = assemblies.First(a => a.FullName.Contains("TestAssembly.A"))!;

        var documents = await XmlDocumentation.LoadAsync(assembly);
        
        Assert.True(true);
    }
}