using System.Xml;

namespace BitzArt.XDoc.Tests;

public class ParsedContentBuilderTests
{
    private readonly ParsedContentBuilder _builder;
    private readonly XDoc _xDoc;

    public ParsedContentBuilderTests()
    {
        _builder = new ParsedContentBuilder();
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
        var result = _builder.Build(typeDoc);

        // Assert
        Assert.Equal("SimpleClass", result.Name);
        Assert.Null(result.Parent);
        Assert.Empty(result.References);
        Assert.NotNull(result.Xml);
    }

    [Fact]
    public void Build_TypeWithInheritDoc_ShouldReturnParsedContentWithParent()
    {
        // Arrange
        var typeDoc = new TypeDocumentation(_xDoc, typeof(DerivedClass), CreateXmlNode("<doc><inheritdoc/></doc>"));

        // Act
        var result = _builder.Build(typeDoc);

        // Assert
        Assert.Equal("DerivedClass", result.Name);
        Assert.NotNull(result.Parent);
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
        var result = _builder.Build(typeDoc);

        // Assert
        Assert.NotEmpty(result.References);
        Assert.Equal("SimpleClass", result.References.Keys.First());
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