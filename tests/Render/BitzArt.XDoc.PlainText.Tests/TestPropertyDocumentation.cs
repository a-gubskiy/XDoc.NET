using System.Xml;
using Moq;

namespace BitzArt.XDoc.Tests;

public class TestPropertyDocumentation : MemberDocumentation
{
    public TestPropertyDocumentation(string xml)
        : base(new XDoc(new DocumentationReferenceResolver()), GetXmlNode(xml))
    {
    }

    private static XmlNode GetXmlNode(string xml)
    {
        var xmlDoc = new XmlDocument();
        xmlDoc.LoadXml(@"<?xml version=""1.0"" encoding=""UTF-8"" standalone=""no""?>" + xml);

        return xmlDoc.DocumentElement!; // Get the node
    }
}

public class FakeMemberDocumentation : MemberDocumentation
{
    public FakeMemberDocumentation(XmlElement element)
        : base(new XDoc(new DocumentationReferenceResolver()), element)
    {
    }

    public FakeMemberDocumentation(XmlText textNode)
        : base(new XDoc(new DocumentationReferenceResolver()), textNode)
    {
    }

    public FakeMemberDocumentation(XmlNode textNode) :
        base(new XDoc(new DocumentationReferenceResolver()), textNode)
    {
    }
}