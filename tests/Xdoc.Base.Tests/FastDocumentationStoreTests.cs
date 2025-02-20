using TestAssembly;
using TestAssembly.A;
using TestAssembly.B;
using Xdoc.Renderer.PlaintText;

namespace Xdoc.Base.Tests;

public class FastDocumentationStoreTests
{
    [Fact]
    public async Task Test1()
    {
        var a = new ClassA();
        var b = new ClassB();

        IDocumentStore documentStore = new DocumentStore();
        
        // var classXmlInfo1 = documentStore.GetClassInfo(typeof(ClassA));
        // var classXmlInfo2 = documentStore.GetClassInfo(typeof(ClassB));
        //
        // var propertyInfo1 = documentStore.GetPropertyInfo(typeof(ClassA), nameof(ClassA.Name));
        //
        // var propertyInfo2 = documentStore.GetPropertyInfo(typeof(ClassA), nameof(ClassA1.Name));
        // var propertyInfo3 = documentStore.GetPropertyInfo(typeof(ClassA), nameof(ClassA2.Name));
        //
        // var propertyInfo4 = documentStore.GetPropertyInfo(typeof(ClassA), nameof(ClassB.Name));

        var x = new ClassB();
        // x.ClassBaseProperty
        
        
        var propertyInfo5 = documentStore.GetPropertyInfo(typeof(ClassB), nameof(ClassB.ClassBaseProperty));

        var renderer = new PlainTextRenderer();

        // var classXmlInfo1Summary = renderer.Render(classXmlInfo1!);
        // var classXmlInfo2Summary =renderer.Render(classXmlInfo2!);
        //
        // var propertyInfo1Summary =renderer.Render(propertyInfo1!);
        // var propertyInfo2Summary =renderer.Render(propertyInfo2!);
        // var propertyInfo3Summary =renderer.Render(propertyInfo3!);
        // var propertyInfo4Summary =renderer.Render(propertyInfo4!);
        
        var propertyInfo5Summary =renderer.Render(propertyInfo5!);

        Assert.True(true);
    }
}