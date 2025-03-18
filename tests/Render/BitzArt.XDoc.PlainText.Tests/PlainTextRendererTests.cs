using System.Xml;
using TestAssembly.A;
using TestAssembly.B;

namespace BitzArt.XDoc.Tests;

public class PlainTextRendererTests
{
    // Act
    // private readonly XDoc _xDoc = new XDoc(new SimpleDocumentationReferenceResolver());
    private readonly XDoc _xDoc = new XDoc();

    [Fact]
    public void Render_PlainTextRenderer_ShouldReturnInheritedProperty()
    {
        // Arrange
        var propertyDocumentation = _xDoc.Get(typeof(Dog).GetProperty(nameof(Dog.Color))!);

        // Act
        var comment = propertyDocumentation.ToPlainText();

        // Assert
        Assert.NotNull(comment);
    }

    [Fact]
    public void Render_PlainTextRenderer_ShouldReturnCrefProperty()
    {
        // Arrange
        var propertyDocumentation = _xDoc.Get(typeof(Dog).GetProperty(nameof(Dog.Field2))!);

        // Act
        var comment = propertyDocumentation.ToPlainText();

        // Assert
        Assert.NotNull(comment);
    }

    [Fact]
    public void Render_PlainTextRenderer_ShouldReturnOtherClassCrefProperty()
    {
        // Arrange
        var propertyDocumentation = _xDoc.Get(typeof(Dog).GetProperty(nameof(Dog.Field3))!);

        // Act
        var comment = propertyDocumentation.ToPlainText();

        // Assert
        Assert.Equal("Field same as Weight", comment);
    }

    [Fact]
    public void Render_PlainTextRenderer_ShouldReturnTypeComment()
    {
        // Arrange
        var propertyDocumentation = _xDoc.Get(typeof(Dog));

        // Act
        var comment = propertyDocumentation.ToPlainText();

        // Assert
        Assert.Equal("This is a class which represents a dog as defined by the interface IDog.", comment);
    }

    [Fact]
    public void Render_PlainTextRenderer_ShouldReturnBaseTypeComment()
    {
        // Arrange
        var propertyDocumentation = _xDoc.Get(typeof(Cat));

        // Act
        var comment = propertyDocumentation.ToPlainText();

        // Assert
        Assert.NotNull(comment);
    }

    [Fact]
    public void Render_PlainTextRenderer_ShouldReturnFormattedText()
    {
        // Arrange
        const string xml = @"<member name=""P:MyCompany.Library.WeeklyMetrics.SuccessRatio"">
                        <summary>
                            The ratio of <see cref=""P:MyCompany.Library.WeeklyMetrics.Progress""/> to <see cref=""P:MyCompany.Library.WeeklyMetrics.Objective""/> for this <see cref=""T:MyCompany.Library.WeeklyMetric""/>.
                        </summary>
                    </member>";

        var memberDocumentation = new TestPropertyDocumentation(xml);

        // Act
        var comment = PlainTextRenderer.Render(memberDocumentation);

        // Assert
        Assert.Contains("The ratio of Progress to Objective for this WeeklyMetric.", comment);
    }


    [Fact]
    public void RenderInheritedDocumentation_PlainTextRenderer_ShouldReturnFormattedText()
    {
        // Arrange
        var propertyDocumentation = _xDoc.Get(typeof(Dog).GetProperty(nameof(Dog.Property1))!);

        // Act
        var text = propertyDocumentation.ToPlainText();

        // Assert
        // Contains("Description of Property1", text);
        Assert.NotNull(text);
    }
}

public class TestPropertyDocumentation : MemberDocumentation
{
    public TestPropertyDocumentation(string xml)
        : base(new XDoc(new SimpleDocumentationReferenceResolver()), GetXmlNode(xml))
    {
    }

    private static XmlNode GetXmlNode(string xml)
    {
        var xmlDoc = new XmlDocument();
        xmlDoc.LoadXml(@"<?xml version=""1.0"" encoding=""UTF-8"" standalone=""no""?>" + xml);

        XmlNode xmlNode = xmlDoc.DocumentElement!; // Get the node

        return xmlNode;
    }
}