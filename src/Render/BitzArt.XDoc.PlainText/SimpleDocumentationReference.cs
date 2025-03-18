using System.Xml;

namespace BitzArt.XDoc;

public class SimpleDocumentationReference : DocumentationReference
{
    public string Cref { get; init; }
    
    public bool IsInheritDoc { get; init; }

    public SimpleDocumentationReference(XmlNode requirementNode, string cref, bool isInheritDoc)
        : base(requirementNode, null)
    {
        Cref = cref;
        IsInheritDoc = isInheritDoc;
    }
}