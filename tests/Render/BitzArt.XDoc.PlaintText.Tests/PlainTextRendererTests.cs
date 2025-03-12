using System.Xml;
using BitzArt.XDoc.PlainText;

namespace BitzArt.XDoc.PlaintText.Tests;

public class PlainTextRendererTests
{
    [Fact]
    public void Render_PlainTextRenderer_ShouldReturnFormattedText()
    {
        // Arrange
        var xml = @"<member name=""P:MyCompany.Library.WeeklyMetrics.SuccessRatio"">
                        <summary>
                            The ratio of <see cref=""P:MyCompany.Library.WeeklyMetrics.Progress""/> to  <see cref=""P:MyCompany.Library.WeeklyMetrics.Objective""/> for this <see cref=""T:MyCompany.Library.WeeklyMetric""/>.
                        </summary>
                    </member>";

        var xmlNode = GetXmlNode(xml);
        var xmlRenderer = new PlainTextRenderer();

        // Act
        var source = new XDoc();
        MemberDocumentation documentation = new TypeDocumentation(source, typeof(object), xmlNode);

        var str = xmlRenderer.Render(documentation);

        // Assert
        Assert.Contains("The ratio of Progress to  Objective for this WeeklyMetric.", str);
    }

    private static XmlNode GetXmlNode(string xml)
    {
        var xmlDoc = new XmlDocument();
        xmlDoc.LoadXml(@"<?xml version=""1.0"" encoding=""UTF-8"" standalone=""no""?>" + xml);

        XmlNode xmlNode = xmlDoc.DocumentElement!; // Get the node

        return xmlNode;
    }
}