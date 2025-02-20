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

        IDocumentStore documentStore = new DocumentStore();
        
        var classXmlInfo1 = documentStore.GetClassInfo(typeof(ClassA));
        var classXmlInfo2 = documentStore.GetClassInfo(typeof(ClassB));

        var propertyInfo1 = documentStore.GetPropertyInfo(typeof(ClassA), nameof(ClassA.Name));
        var propertyInfo2 = documentStore.GetPropertyInfo(typeof(ClassA), nameof(ClassB.Name));
        


        // var assemblies = AppDomain.CurrentDomain.GetAssemblies()
        //     .Where(a => a.FullName?.StartsWith("TestAssembly.") ?? false)
        //     .ToList();
        //
        // var documents = await XmlDocumentation.LoadAsync(assemblies);
        //
        // var documentationStore = new FastDocumentationStore(documents);
        //
        // var commentForProperty = documentationStore.GetCommentForProperty(typeof(ClassA1), nameof(ClassA1.Field1));

        Assert.True(true);
    }
}