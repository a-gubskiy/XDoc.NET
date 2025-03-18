using System.Xml;

namespace BitzArt.XDoc;

public class SimpleDocumentationReference : DocumentationReference
{
    public string TypeName { get; }
    
    public string? MemberName { get; }
 
    public SimpleDocumentationReference(XmlNode requirementNode, string typeName, string? memberName)
        : base(requirementNode, null)
    {
        TypeName = typeName;
        MemberName = memberName;
    }
}