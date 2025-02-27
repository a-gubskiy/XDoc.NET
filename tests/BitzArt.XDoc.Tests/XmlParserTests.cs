using System.Xml;

namespace BitzArt.XDoc.Tests;

public class TestClass
{
    public int MyProperty { get; set; }
}

public class XmlParserTests
{
    [Fact]
    public void Parse_XmlDocWithTypeNode_ShouldReturnTypeDocumentation()
    {
        // Arrange
        var assembly = GetType().Assembly;

        var content = "blah";

        var testType = typeof(TestClass);
        var testNode = new TestMemberNode(testType, content);

        var xml = new TestMemberNode[] { testNode }.GetXml(assembly);

        var doc = new XmlDocument();
        doc.LoadXml(xml);

        var parser = new XmlParser(new XDoc(), assembly, doc);

        // Act
        var results = parser.Parse();

        // Assert
        Assert.NotNull(results);
        Assert.Single(results);

        var entry = results.First();
        var type = entry.Key;
        var documentation = entry.Value;

        Assert.Equal(typeof(TestClass), type);

        Assert.NotNull(documentation);
        Assert.Empty(documentation.MemberData);

        var fetchedXmlData = documentation.Node!.InnerXml;

        // `Trim` since the XML data is formatted with newlines and spaces
        var trimmed = fetchedXmlData.Trim();
        Assert.Equal(content, trimmed);
    }

    [Fact]
    public void Parse_XmlDocWithPropertyNode_ShouldReturnPropertyDocumentationInsideTypeDocumentation()
    {
        // Arrange
        var assembly = GetType().Assembly;

        var content = "blah";

        var testType = typeof(TestClass);
        var testProperty = testType.GetProperty(nameof(TestClass.MyProperty))!;
        var testNode = new TestMemberNode(testProperty, content);

        var xml = new TestMemberNode[] { testNode }.GetXml(assembly);

        var doc = new XmlDocument();
        doc.LoadXml(xml);

        var parser = new XmlParser(new XDoc(), assembly, doc);

        // Act
        var results = parser.Parse();

        // Assert
        Assert.NotNull(results);

        // `Single` because only 1 type is used - it's member data is inside of it
        Assert.Single(results);

        var typeDocs = results.First().Value;

        Assert.NotNull(typeDocs.MemberData);
        Assert.Single(typeDocs.MemberData);

        var member = typeDocs.MemberData.First();
        
        Assert.Equal(testProperty, member.Key);

        Assert.NotNull(member.Value);
        Assert.IsType<PropertyDocumentation>(member.Value);
        var memberDocumentation = (PropertyDocumentation)member.Value;

        var fetchedXmlData = memberDocumentation.Node!.InnerXml;

        // `Trim` since the XML data is formatted with newlines and spaces
        var trimmed = fetchedXmlData.Trim();
        Assert.Equal(content, trimmed);
    }
}
