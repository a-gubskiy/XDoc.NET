using System.Xml;

namespace BitzArt.XDoc.PlainText.Tests;

public class TestDocumentationElement : IMemberDocumentation
{
    public TestDocumentationElement(XmlElement element) => Node = element;

    public TestDocumentationElement(XmlText textNode) => Node = textNode;

    public TestDocumentationElement(XmlNode textNode) => Node = textNode;

    public XmlNode? Node { get; }
}