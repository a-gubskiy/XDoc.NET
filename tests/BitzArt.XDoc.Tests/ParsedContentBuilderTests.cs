using System.Reflection;
using System.Xml;
using TestAssembly.B;

namespace BitzArt.XDoc.Tests;

public class ParsedContentBuilderTests
{
    [Fact]
    public void Build__LoadListParsedContent()
    {
        string path =
            "/Users/andrew/Projects/MediaMars/xdoc/tests/MultiAssemblyTests/TestAssembly.B/bin/Debug/net8.0/TestAssembly.B.xml";
        var xml = File.ReadAllText(path);

        var doc = new XmlDocument();
        doc.LoadXml(xml);

        var assembly = Assembly.GetAssembly(typeof(Dog));

        var parser = new XmlParser(new XDoc(), assembly, doc);

        // Act
        var docs = parser.Parse();
        var (type, documentation) = docs.FirstOrDefault();

        var parsedContentBuilder = new ParsedContentBuilder();

        var parsedContent = parsedContentBuilder.Build(documentation);
        
        Assert.NotNull(parsedContent);


        // var xDoc = new XDoc();
        //
        // var type = typeof(object);
        //
        // Assert.Throws<XDocException>(() =>
        // {
        //     var documentation = xDoc.Get(type);
        // });
    }
}