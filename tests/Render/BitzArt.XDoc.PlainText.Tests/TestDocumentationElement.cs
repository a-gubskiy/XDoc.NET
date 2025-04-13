using System.Reflection;
using System.Xml;

namespace BitzArt.XDoc.PlainText.Tests;

public class TestDocumentationElement : IMemberDocumentation
{
    public TestDocumentationElement(XmlElement element) => Node = element;

    public TestDocumentationElement(XmlText textNode) => Node = textNode;

    public TestDocumentationElement(XmlNode textNode) => Node = textNode;

    public MemberInfo Member { get; }
    
    public XmlNode? Node { get; }
    
    public IMemberDocumentation? GetInheritanceTargetDocumentation()
    {
        throw new NotImplementedException();
    }

    public MemberInfo? GetInheritanceTarget()
    {
        throw new NotImplementedException();
    }
}