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
        var xmlDoc = new XmlDocument();
        var node = xmlDoc.CreateElement("member");
        node.InnerXml = "<summary>See <see cref=\"T:System.String\"/> for details.</summary>";

        var documentation = new TestMemberDocumentation
        {
            Node = node
        };

        // Act
        var result = CrefResolver.Resolve(documentation);

        // Assert
        Assert.Single(result);
        
        /*var reference = result.First();
        
        Assert.Equal("System.String", reference.Target);*/
    }

    [Fact]
    public void Resolve_WithMultipleSeeElements_ReturnsDistinctReferences()
    {
        // Arrange
        var xDoc = new XDoc();

        var propertyDocumentation = xDoc.Get(typeof(Dog).GetProperty(nameof(Dog.Name)));

        // var xmlDoc = new XmlDocument();
        //
        // var node = xmlDoc.CreateElement("member");
        //
        // node.InnerXml = @"
        //     <summary>
        //         See <see cref=""T:System.String""/> and
        //         <see cref=""T:System.Int32""/> and
        //         <see cref=""T:System.String""/> for details.
        //     </summary>";
        //
        // var documentation = new TestMemberDocumentation
        // {
        //     Node = node,
        //     Source = xDoc
        // };

        // Act
        var result = CrefResolver.Resolve(propertyDocumentation);

        // Assert
        Assert.Equal(2, result.Count);
        // Assert.Contains(result, r => r.Target == "System.String");
        // Assert.Contains(result, r => r.Target == "System.Int32");
    }

    [Fact]
    public void Resolve_WithInvalidCrefAttribute_SkipsInvalidReference()
    {
        // Arrange
        var xDoc = new XDoc();
        var xmlDoc = new XmlDocument();
        
        var node = xmlDoc.CreateElement("member");
        
        node.InnerXml = @"
            <summary>
                See <see cref=""""/> and
                <see cref=""T:System.String""/> for details.
            </summary>";

        var documentation = new TestMemberDocumentation
        {
            Node = node,
            Source = xDoc
        };

        // Act
        var result = CrefResolver.Resolve(documentation);

        // Assert
        Assert.Single(result);
        
        // var reference = result.First();
        // Assert.Equal("System.String", reference.Target);
    }

    private class TestMemberDocumentation : MemberDocumentation
    {
    }
}