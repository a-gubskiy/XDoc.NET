using System.Xml;
using BitzArt.XDoc.Resolvers;
using TestAssembly.B;

namespace BitzArt.XDoc.Tests;

public class CrefResolverTests
{
    private readonly XDoc _xdoc;

    public CrefResolverTests()
    {
        _xdoc = new XDoc();
    }

    [Fact]
    public void Resolve_WithNullNode_ReturnsEmptyCollection()
    {
        // Arrange
        var documentation = new TestMemberDocumentation(null!, null!);

        // Act
        var result = CrefResolver.Resolve(documentation);

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public void Resolve_WithEmptyXml_ReturnsEmptyCollection()
    {
        // Arrange
        var xmlDoc = new XmlDocument();
        var node = xmlDoc.CreateElement("member");

        var documentation = new TestMemberDocumentation(null!, node);

        // Act
        var result = CrefResolver.Resolve(documentation);

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public void Resolve_WithNoSeeElements_ReturnsEmptyCollection()
    {
        // Arrange
        var xmlDoc = new XmlDocument();
        var node = xmlDoc.CreateElement("member");
        node.InnerXml = "<summary>Test documentation</summary>";

        var documentation = new TestMemberDocumentation(null!, node);

        // Act
        var result = CrefResolver.Resolve(documentation);

        // Assert
        Assert.Empty(result);
    }


    [Fact]
    public void Resolve_WithMultipleSeeElements_ReturnsDistinctReferences()
    {
        // Arrange
        var xDoc = new XDoc();

        var propertyDocumentation = xDoc.Get(typeof(Dog).GetProperty(nameof(Dog.Name)));

        // Act
        var result = CrefResolver.Resolve(propertyDocumentation!);

        // Assert
        Assert.Equal(2, result.Count);
    }

    [Fact]
    public void Resolve_WithInvalidCrefAttribute_SkipsInvalidReference()
    {
        // Arrange
        var xDoc = new XDoc();

        var propertyDocumentation = xDoc.Get(typeof(Dog).GetProperty(nameof(Dog.PropertyWIthInalidCref)));

        // Act
        var result = CrefResolver.Resolve(propertyDocumentation!);
        
        //Assert
        Assert.Empty(result);
    }

    private class TestMemberDocumentation : MemberDocumentation
    {
        public TestMemberDocumentation(IXDoc source, XmlNode node)
            : base(source, node)
        {
        }
    }
}