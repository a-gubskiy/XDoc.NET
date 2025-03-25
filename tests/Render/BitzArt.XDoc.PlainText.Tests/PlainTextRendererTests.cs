using System.Xml;

namespace BitzArt.XDoc.Tests;

public class PlainTextRendererTests
{
    private PlainTextRenderer _plainTextRenderer = new PlainTextRenderer();
    
    [Fact]
    public void Render_ReturnsEmptyString_WhenDocumentationIsNull()
    {
        // Act
        var result = _plainTextRenderer.Render(null);

        // Assert
        Assert.Equal(string.Empty, result);
    }

    [Fact]
    public void Render_ReturnsPlainText_WhenTextNodeProvided()
    {
        // Arrange
        var textNode = new XmlDocument().CreateTextNode("Hello World");
        var memberDocumentation = new FakeMemberDocumentation(textNode);

        // Act
        var result = _plainTextRenderer.Render(memberDocumentation);

        // Assert
        Assert.Equal("Hello World", result);
    }

    [Fact]
    public void Render_NormalizesMultipleLinesToNewlines_WhenForceSingleLineIsFalse()
    {
        // Arrange
        var element = new XmlDocument().CreateElement("para");
        element.InnerXml = "Line1\n   Line2\nLine3";

        var memberDocumentation = new FakeMemberDocumentation(element);

        // Act
        var result = _plainTextRenderer.Render(memberDocumentation);

        // Assert
        Assert.Equal("Line1\nLine2\nLine3", result);
    }

    [Fact]
    public void Render_NormalizesMultipleLinesToSingleLine_WhenForceSingleLineIsTrue()
    {
        // Arrange
        var element = new XmlDocument().CreateElement("para");
        element.InnerXml = "Line1\n   Line2\nLine3";

        var memberDocumentation = new FakeMemberDocumentation(element);

        // Act
        var result = _plainTextRenderer.Render(memberDocumentation);

        // Assert
        Assert.Equal("Line1 Line2 Line3", result);
    }

    [Fact]
    public void Render_RendersCrefReference_ReturnsShortTypeName()
    {
        var xmlDocument = new XmlDocument();
        XmlNode xmlNode = xmlDocument.CreateNode(XmlNodeType.Element, "test", null);


        // Arrange
        var element = xmlDocument.CreateElement("see");
        element.SetAttribute("cref", "T:BitzArt.XDoc.Models.SomeType");
        xmlNode.AppendChild(element);

        xmlDocument.AppendChild(xmlNode);


        var memberDocumentation = new FakeMemberDocumentation(xmlNode);

        // Act
        var result = _plainTextRenderer.Render(memberDocumentation);

        // Assert
        Assert.Equal("SomeType", result);
    }
    
    [Fact]
    public void Render_RendersCrefReference_ReturnsMethodName()
    {
        var xmlDocument = new XmlDocument();
        XmlNode xmlNode = xmlDocument.CreateNode(XmlNodeType.Element, "test", null);


        // Arrange
        var element = xmlDocument.CreateElement("see");
        element.SetAttribute("cref", "M:BitzArt.XDoc.Models.SomeType.SomeMethod");
        xmlNode.AppendChild(element);

        xmlDocument.AppendChild(xmlNode);


        var memberDocumentation = new FakeMemberDocumentation(xmlNode);

        // Act
        var result = _plainTextRenderer.Render(memberDocumentation);

        // Assert
        Assert.Equal("SomeType.SomeMethod", result);
    }

    [Fact]
    public void Render_ReturnsEmptyString_WhenXmlElementIsEmpty()
    {
        // Arrange
        var element = new XmlDocument().CreateElement("empty");
        var memberDocumentation = new FakeMemberDocumentation(element);

        // Act
        var result = _plainTextRenderer.Render(memberDocumentation);

        // Assert
        Assert.Equal(string.Empty, result);
    }
}