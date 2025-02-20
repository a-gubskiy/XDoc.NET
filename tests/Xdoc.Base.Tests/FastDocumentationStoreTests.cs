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
        
        var propertyInfo2 = documentStore.GetPropertyInfo(typeof(ClassA), nameof(ClassA1.Name));
        var propertyInfo3 = documentStore.GetPropertyInfo(typeof(ClassA), nameof(ClassA2.Name));
        
        var propertyInfo4 = documentStore.GetPropertyInfo(typeof(ClassA), nameof(ClassB.Name));
        
        Assert.True(true);
    }
}