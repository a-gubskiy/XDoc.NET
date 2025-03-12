using System.Xml;
using BitzArt.XDoc.Resolvers;

namespace BitzArt.XDoc.Tests;

public class InheritanceResolverTests
{
    [Fact]
    public void Resolve_WithNullNode_ReturnsNull()
    {
        // Arrange
        var documentation = new TestMemberDocumentation(null!, null!);

        // Act
        var result = InheritanceResolver.Resolve(documentation);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void Resolve_WithEmptyXml_ReturnsNull()
    {
        // Arrange
        var xmlDoc = new XmlDocument();
        var node = xmlDoc.CreateElement("member");
        var documentation = new TestMemberDocumentation(null!, node);

        // Act
        var result = InheritanceResolver.Resolve(documentation);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void Resolve_WithNoInheritDoc_ReturnsNull()
    {
        // Arrange
        var xmlDoc = new XmlDocument();
        var node = xmlDoc.CreateElement("member");
        node.InnerXml = "<summary>Test documentation</summary>";
        var documentation = new TestMemberDocumentation(null!, node);

        // Act
        var result = InheritanceResolver.Resolve(documentation);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void Resolve_WithInheritDoc_ReturnsReference()
    {
        // Arrange
        var xmlDoc = new XmlDocument();
        var node = xmlDoc.CreateElement("member");
        node.InnerXml = "<inheritdoc/>";
        
        var documentation = new TestMemberDocumentation(null!, node);

        // Act
        var result = InheritanceResolver.Resolve(documentation);

        // Assert
        Assert.NotNull(result);
        Assert.IsType<InheritanceMemberDocumentationReference>(result);
    }

    private class TestMemberDocumentation : MemberDocumentation
    {
        public TestMemberDocumentation(XDoc source, XmlNode node)
            : base(source, node)
        {
        }
    }
}