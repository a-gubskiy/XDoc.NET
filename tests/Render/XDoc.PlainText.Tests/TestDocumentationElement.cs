using System.Reflection;
using System.Xml;

namespace XDoc.PlainText.Tests;

public class TestDocumentationElement : IMemberDocumentation
{
    public TestDocumentationElement(XmlNode node)
    {
        Node = node;
    }

    public MemberInfo Member { get; }

    public XmlNode? Node { get; }

    public IMemberDocumentation? GetInheritanceTargetDocumentation() => null;

    public MemberInfo? GetInheritanceTarget() => null;
}