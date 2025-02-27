using System.Xml;

namespace BitzArt.XDoc.Tests;

public class ParsedContentResolverTests
{
    private readonly XDoc _xDoc;

    public ParsedContentResolverTests()
    {
        _xDoc = new XDoc();
    }

    [Fact]
    public void Build_TypeWithoutReferences_ShouldReturnBasicParsedContent()
    {
        // Arrange
        var typeDoc = new TypeDocumentation(
            _xDoc, 
            typeof(SimpleClass),
            CreateXmlNode("<doc><summary>Simple class</summary></doc>"));

        // Act
        var result = ParsedContentResolver.Resolve(typeDoc);

        // Assert
        Assert.Equal("SimpleClass", result.Name);
        Assert.Null(result.InheritedContent);
        Assert.Empty(result.GetReferences());
        Assert.NotNull(result.Xml);
    }

    [Fact]
    public void Build_TypeWithInheritDoc_ShouldReturnParsedContentWithParent()
    {
        // Arrange
        var typeDoc = new TypeDocumentation(_xDoc, typeof(DerivedClass), CreateXmlNode("<doc><inheritdoc/></doc>"));

        // Act
        var result = ParsedContentResolver.Resolve(typeDoc);

        // Assert
        Assert.Equal("DerivedClass", result.Name);
        Assert.NotNull(result.InheritedContent);
    }

    [Fact]
    public void Build_TypeWithSeeReference_ShouldReturnParsedContentWithReferences()
    {
        // Arrange
        var typeDoc = new TypeDocumentation(
            _xDoc,
            typeof(ClassWithReferences),
            CreateXmlNode("<doc><summary>See <see cref=\"T:SimpleClass\"/></summary></doc>"));

        // Act
        var result = ParsedContentResolver.Resolve(typeDoc);

        // Assert
        Assert.NotEmpty(result.GetReferences());
        Assert.Equal("SimpleClass", result.GetReferences().Keys.First());
    }
    
    private XmlNode CreateXmlNode(string xml)
    {
        var doc = new XmlDocument();
        doc.LoadXml(xml);
        return doc.DocumentElement!;
    }
}

// Test helper classes
public class SimpleClass
{
}

public class DerivedClass : SimpleClass
{
}

public class ClassWithReferences
{
}

public interface ITestInterface
{
    string Name { get; }
}

public class ImplementingClass : ITestInterface
{
    public string Name => "";
}