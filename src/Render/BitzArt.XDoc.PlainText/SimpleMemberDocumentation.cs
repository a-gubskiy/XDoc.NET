using System.Xml;

namespace BitzArt.XDoc;

public class SimpleMemberDocumentation : MemberDocumentation
{
    public SimpleMemberDocumentation(XDoc source, XmlNode? node)
        : base(source, node)
    {
    }
}