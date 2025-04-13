using System.Reflection;
using System.Xml;

namespace BitzArt.XDoc.PlainText.Tests;

public class TestDocumentationElement : IMemberDocumentation
{
    public TestDocumentationElement(XmlNode node)
    {
        Node = node;
    }

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