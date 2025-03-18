using System.Xml;

namespace BitzArt.XDoc;

public class SimpleDocumentationReferenceResolver : DocumentationReferenceResolver
{
    /// <inheritdoc/>
    protected override DocumentationReference? GetInheritReference(XDoc source, XmlNode node)
    {
        var attribute = node.ParentNode?.Attributes?["name"];

        //P:TestAssembly.B.Dog.Color
        var referenceName = attribute?.Value ?? string.Empty;
        var (prefix, typeName, memberName) = GetTypeAndMember(referenceName);

        return new SimpleDocumentationReference(node, typeName, memberName);
    }

    /// <inheritdoc/>
    protected override DocumentationReference? GetCrefReference(XDoc source, XmlNode node, XmlAttribute? attribute)
    {
        // P:TestAssembly.B.Dog.Name
        var referenceName = attribute?.Value ?? string.Empty;
        var (prefix, typeName, memberName) = GetTypeAndMember(referenceName);

        return new SimpleDocumentationReference(node, typeName, memberName);
    }
}