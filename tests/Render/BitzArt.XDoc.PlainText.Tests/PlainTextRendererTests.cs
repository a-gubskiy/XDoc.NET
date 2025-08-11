using BitzArt.XDoc.Tests;
using System.Xml;

namespace BitzArt.XDoc.PlainText.Tests;

public class TestClass
{
    public int MyProperty { get; set; }
}

public class PlainTextRendererTests
{
    [Fact]
    public void Render_PropertyWithSummary_ShouldRenderSummary()
    {
        // Arrange
        var assembly = GetType().Assembly;

        var property = typeof(TestClass).GetProperty(nameof(TestClass.MyProperty));

        var myContent = "some text";
        var propertyNode = new TestMemberNode(property!, myContent);

        var nodes = new List<TestMemberNode>()
        {
            propertyNode
        };

        var xml = nodes.GetXml(assembly);
        var xmlDocument = new XmlDocument();
        xmlDocument.LoadXml(xml);

        var node = xmlDocument.SelectSingleNode("//member[@name='P:XDoc.PlainText.Tests.TestClass.MyProperty']");

        var memberDocumentation = new TestDocumentationElement(node!);

        var plainTextRenderer = new PlainTextRenderer();
        var comment = plainTextRenderer.Render(memberDocumentation);

        Assert.Equal(myContent, comment);
    }

    [Fact]
    public void Render_ReturnsEmptyString_WhenInheritDocTagProvided()
    {
        // Arrange
        var assembly = GetType().Assembly;

        List<TestMemberNode> nodes =
        [
            new TestMemberNode(typeof(TestClass), "node with inherit doc test"),
            new TestMemberNode(TestNodeType.Property,
                "XDoc.Tests.TestClass.Name",
                "<member name=\"P:XDoc.Tests.TestClass.Name\"><inheritdoc /></member>")
        ];

        var xml = nodes.GetXml(assembly);
        var xmlDocument = new XmlDocument();
        xmlDocument.LoadXml(xml);

        var node = xmlDocument.SelectSingleNode("//inheritdoc")!;

        var memberDocumentation = new TestDocumentationElement(node);

        var plainTextRenderer = new PlainTextRenderer();
        var comment = plainTextRenderer.Render(memberDocumentation);

        Assert.Equal(string.Empty, comment);
    }

    [Fact]
    public void Render_ReturnsEmptyString_WhenDocumentationIsNull()
    {
        // Assert
        Assert.Throws<ArgumentNullException>(() =>
        {
            var result = new PlainTextRenderer().Render(null);
        });
    }

    [Fact]
    public void Render_ReturnsPlainText_WhenTextNodeProvided()
    {
        // Arrange
        var textNode = new XmlDocument().CreateTextNode("Hello World");
        var memberDocumentation = new TestDocumentationElement(textNode);
        var plainTextRenderer = new PlainTextRenderer();

        // Act
        var result = plainTextRenderer.Render(memberDocumentation);

        // Assert
        Assert.Equal("Hello World", result);
    }

    [Fact]
    public void Render_NormalizesMultipleLinesToSingleLine_WhenForceSingleLineIsTrue()
    {
        // Arrange
        var element = new XmlDocument().CreateElement("para");
        element.InnerXml = "Line1\n   Line2\nLine3";

        var memberDocumentation = new TestDocumentationElement(element);
        var plainTextRenderer = new PlainTextRenderer();

        // Act
        var result = plainTextRenderer.Render(memberDocumentation);

        // Assert
        Assert.Equal("Line1\nLine2\nLine3", result);
    }

    [Fact]
    public void Render_RendersCrefReference_ReturnsShortTypeName()
    {
        // Arrange
        var xmlDocument = new XmlDocument();
        var xmlNode = xmlDocument.CreateNode(XmlNodeType.Element, "test", null);
        var element = xmlDocument.CreateElement("see");

        element.SetAttribute("cref", "T:XDoc.Models.SomeType");
        xmlNode.AppendChild(element);

        xmlDocument.AppendChild(xmlNode);

        var memberDocumentation = new TestDocumentationElement(xmlNode);
        var plainTextRenderer = new PlainTextRenderer();

        // Act
        var result = plainTextRenderer.Render(memberDocumentation);

        // Assert
        Assert.Equal("SomeType", result);
    }

    [Fact]
    public void Render_RendersCrefReference_ReturnsMethodName()
    {
        // Arrange
        var xmlDocument = new XmlDocument();
        var xmlNode = xmlDocument.CreateNode(XmlNodeType.Element, "test", null);
        var element = xmlDocument.CreateElement("see");

        element.SetAttribute("cref", "M:XDoc.Models.SomeType.SomeMethod");
        xmlNode.AppendChild(element);

        xmlDocument.AppendChild(xmlNode);

        var memberDocumentation = new TestDocumentationElement(xmlNode);
        var plainTextRenderer = new PlainTextRenderer();

        // Act
        var result = plainTextRenderer.Render(memberDocumentation);

        // Assert
        Assert.Equal("SomeType.SomeMethod", result);
    }

    [Fact]
    public void Render_ReturnsEmptyString_WhenXmlElementIsEmpty()
    {
        // Arrange
        var element = new XmlDocument().CreateElement("empty");
        var memberDocumentation = new TestDocumentationElement(element);
        var plainTextRenderer = new PlainTextRenderer();

        // Act
        var result = plainTextRenderer.Render(memberDocumentation);

        // Assert
        Assert.Equal(string.Empty, result);
    }
}