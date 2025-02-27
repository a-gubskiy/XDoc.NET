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
        var documentation = new TestMemberDocumentation
        {
            Node = null
        };

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

        var documentation = new TestMemberDocumentation
        {
            Node = node
        };

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

        var documentation = new TestMemberDocumentation
        {
            Node = node
        };

        // Act
        var result = CrefResolver.Resolve(documentation);

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public void Resolve_WithSingleSeeElement_ReturnsSingleReference()
    {
        // Arrange
        var xDoc = new XDoc();

        var propertyDocumentation = xDoc.Get(typeof(Dog).GetProperty(nameof(Dog.PropertyWIthSingleSee)));

        // Act
        var result = CrefResolver.Resolve(propertyDocumentation);

        // Assert
        Assert.Single(result);

        var reference = result.First();

        Assert.Equal("System.String", reference.ReferencedType);
    }

    [Fact]
    public void Resolve_WithMultipleSeeElements_ReturnsDistinctReferences()
    {
        // Arrange
        var xDoc = new XDoc();

        var propertyDocumentation = xDoc.Get(typeof(Dog).GetProperty(nameof(Dog.Name)));

        // Act
        var result = CrefResolver.Resolve(propertyDocumentation);

        // Assert
        Assert.Equal(2, result.Count);

        Assert.Contains(result, r => r.ReferencedType == "TestAssembly.B.Dog");
        Assert.Contains(result, r => r.ReferencedType == "TestAssembly.A.Animal");
    }

    [Fact]
    public void Resolve_WithInvalidCrefAttribute_SkipsInvalidReference()
    {
        // Arrange
        var xDoc = new XDoc();

        var propertyDocumentation = xDoc.Get(typeof(Dog).GetProperty(nameof(Dog.PropertyWIthInalidCref)));

        // Act + Assert
        Assert.Throws<NullReferenceException>(() =>
        {
            var result = CrefResolver.Resolve(propertyDocumentation);
        });
    }

    private class TestMemberDocumentation : MemberDocumentation
    {
    }
}