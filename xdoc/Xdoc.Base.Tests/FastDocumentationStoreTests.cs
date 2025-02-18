using TestAssembly;
using TestAssembly.A;
using TestAssembly.B;

namespace Xdoc.Base.Tests;

public class FastDocumentationStoreTests
{
    [Fact]
    public async Task Test1()
    {
        var a = new ClassA();
        var b = new ClassB();
        
        
        var assemblies = AppDomain.CurrentDomain.GetAssemblies()
            .Where(a => a.FullName?.StartsWith("TestAssembly.") ?? false)
            .ToList();

        var documents = await XmlDocumentation.LoadAsync(assemblies);

        var documentationStore = new FastDocumentationStore(documents);

        var commentForProperty = documentationStore.GetCommentForProperty(typeof(ClassA1), nameof(ClassA1.Field1));

        Assert.True(true);
    }
}