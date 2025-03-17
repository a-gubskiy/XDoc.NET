using System.Xml;

namespace BitzArt.XDoc;

public class SimpleDocumentationReferenceResolver : DocumentationReferenceResolver, IDocumentationReferenceResolver
{
    /// <inheritdoc/>
    protected override DocumentationReference? GetInheritReference(XDoc source, XmlNode node)
    {
        var attribute = node.ParentNode?.Attributes?["name"];

        //P:TestAssembly.B.Dog.Color
        var referenceName = attribute?.Value ?? string.Empty;
        var (prefix, typeName, memberName) = GetTypeAndMember(referenceName);

        // var type = GetType(typeName);
        // var baseType = type.BaseType;
        // var baseTypeName = GetBaseTypeName(typeName);

        MemberDocumentation? targetDocumentation = null;

        if (prefix is "T:")
        {
            // We do not know how to get the base type documentation
            targetDocumentation = new SimpleMemberDocumentation(source, null);
        }
        else if (prefix is "P:" or "M:" or "F:")
        {
            // We do not know how to get the member inheritance documentation
            targetDocumentation = new SimpleMemberDocumentation(source, null);
        }

        if (targetDocumentation == null)
        {
            return null;
        }

        return new DocumentationReference(node, targetDocumentation);
    }

    /// <inheritdoc/>
    protected override DocumentationReference? GetCrefReference(XDoc source, XmlNode node, XmlAttribute? attribute)
    {
        // P:TestAssembly.B.Dog.Name
        var referenceName = attribute?.Value ?? string.Empty;
        var (prefix, typeName, memberName) = GetTypeAndMember(referenceName);

        MemberDocumentation? targetDocumentation = null;

        if (prefix is "T:")
        {
            // We don't know how to get the type documentation
            targetDocumentation = new SimpleMemberDocumentation(source, null);
        }
        else if (prefix is "P:" or "M:" or "F:")
        {
            // We don't know how to get the member documentation
            targetDocumentation = new SimpleMemberDocumentation(source, null);
        }

        if (targetDocumentation == null)
        {
            return null;
        }

        return new DocumentationReference(node, targetDocumentation);
    }
}