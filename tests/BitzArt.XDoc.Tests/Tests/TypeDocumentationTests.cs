using System.Reflection;
using System.Xml;

namespace BitzArt.XDoc.Tests;

public class TypeDocumentationTestClass
{
    public int MyProperty { get; set; }
}

public class InheritedTypeDocumentationTestClass : TypeDocumentationTestClass
{
}

public class TypeDocumentationTests
{
    [Fact]
    public void GetDocumentation_PropertyInfo_ReturnsPropertyDocumentation()
    {
        var source = new XDoc();
        
        var xmlDocument = new XmlDocument();
        var node = xmlDocument.CreateTextNode("blah");
        var propertyInfo = typeof(TypeDocumentationTestClass).GetProperty(nameof(TypeDocumentationTestClass.MyProperty));

        var typeDocumentation = new TypeDocumentation(source, typeof(TypeDocumentationTestClass), node);

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
        var inheritedPropertyInfo = typeof(InheritedTypeDocumentationTestClass).GetProperty(nameof(InheritedTypeDocumentationTestClass.MyProperty))!;
        var propertyInfo = typeof(TypeDocumentationTestClass).GetProperty(nameof(TypeDocumentationTestClass.MyProperty))!;
        
        var typeDocumentation = new TypeDocumentation(source, typeof(TypeDocumentationTestClass), node);

        var inheritedPropertyDocumentation = new PropertyDocumentation(source, inheritedPropertyInfo, node);
        var propertyDocumentation  = new PropertyDocumentation(source, propertyInfo, node);;
        
        typeDocumentation.AddMemberData(inheritedPropertyInfo, inheritedPropertyDocumentation);
        typeDocumentation.AddMemberData(propertyInfo, propertyDocumentation);

        var resolvedPropertyDocumentation = typeDocumentation.GetDocumentation(inheritedPropertyInfo);

        Assert.Same(propertyDocumentation, resolvedPropertyDocumentation);
        Assert.NotNull(resolvedPropertyDocumentation);
    }
}