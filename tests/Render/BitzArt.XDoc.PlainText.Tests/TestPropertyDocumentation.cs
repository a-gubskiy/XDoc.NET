using System.Xml;
using Moq;

namespace BitzArt.XDoc.Tests;

public class FakeMemberDocumentation : DocumentationElement
{
    public FakeMemberDocumentation(XmlElement element)
        : base(new XDoc(), element)
    {
    }

    public FakeMemberDocumentation(XmlText textNode)
        : base(new XDoc(), textNode)
    {
    }

    public FakeMemberDocumentation(XmlNode textNode) :
        base(new XDoc(), textNode)
    {
    }
}