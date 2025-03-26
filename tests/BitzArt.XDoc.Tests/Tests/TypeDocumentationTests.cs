using System.Xml;

namespace BitzArt.XDoc.Tests;

public class TypeDocumentationTests
{
    public class TestClass
    {
        public int MyProperty { get; set; }
    }

    public class InheritedTestClass : TestClass
    {
    }
    
    [Fact]
    public void GetDocumentation_PropertyInfo_ReturnsPropertyDocumentation()
    {
        var source = new XDoc();

        var xmlDocument = new XmlDocument();
        var node = xmlDocument.CreateTextNode("blah");
        var propertyInfo = typeof(TestClass).GetProperty(nameof(TestClass.MyProperty))!;
        var typeDocumentation = new TypeDocumentation(source, typeof(TestClass), node);

        var propertyDocumentation = new PropertyDocumentation(source, propertyInfo, node);
        typeDocumentation.AddMemberData(propertyInfo, propertyDocumentation);

        var resolvedPropertyDocumentation = typeDocumentation.GetDocumentation(propertyInfo);

        Assert.Same(propertyDocumentation, resolvedPropertyDocumentation);
        Assert.NotNull(resolvedPropertyDocumentation);
    }

    [Fact]
    public void GetDocumentation_InheritedPropertyInfo_ReturnsPropertyDocumentation()
    {
        var source = new XDoc();

        var xmlDocument = new XmlDocument();
        var node = xmlDocument.CreateTextNode("blah");
        var inheritedPropertyInfo = typeof(InheritedTestClass).GetProperty(nameof(InheritedTestClass.MyProperty))!;
        var propertyInfo = typeof(TestClass).GetProperty(nameof(TestClass.MyProperty))!;

        var propertyDocumentation = new PropertyDocumentation(source, propertyInfo, node);
        var inheritedPropertyDocumentation = new PropertyDocumentation(source, inheritedPropertyInfo, node);

        var typeDocumentation = new TypeDocumentation(source, typeof(TestClass), node);
        typeDocumentation.AddMemberData(inheritedPropertyInfo, inheritedPropertyDocumentation);
        typeDocumentation.AddMemberData(propertyInfo, propertyDocumentation);

        var resolvedPropertyDocumentation = typeDocumentation.GetDocumentation(inheritedPropertyInfo);

        Assert.Same(propertyDocumentation, resolvedPropertyDocumentation);
        Assert.NotNull(resolvedPropertyDocumentation);
    }

    [Fact]
    public void GetDocumentation_InheritedPropertyInfo_ShouldThorException()
    {
        var source = new XDoc();

        var xmlDocument = new XmlDocument();
        var node = xmlDocument.CreateTextNode("blah");
        var inheritedPropertyInfo = typeof(InheritedTestClass).GetProperty(nameof(InheritedTestClass.MyProperty))!;

        var typeDocumentation = new TypeDocumentation(source, typeof(InheritedTestClass), node);

        var inheritedPropertyDocumentation = new PropertyDocumentation(source, inheritedPropertyInfo, node);

        typeDocumentation.AddMemberData(inheritedPropertyInfo, inheritedPropertyDocumentation);

        Assert.ThrowsAny<Exception>(() =>
        {
            var resolvedPropertyDocumentation = typeDocumentation.GetDocumentation(inheritedPropertyInfo);
        });

    }
}