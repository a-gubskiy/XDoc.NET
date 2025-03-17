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

        // var type = GetType(typeName);
        // var baseType = type.BaseType;
        // var baseTypeName = GetBaseTypeName(typeName);

        MemberDocumentation? targetDocumentation = null;

        if (prefix is "T:")
        {
            targetDocumentation = GetSimpleMemberDocumentation(source, null);
        }
        else if (prefix is "P:" or "M:" or "F:")
        {
            targetDocumentation = GetSimpleMemberDocumentation(source, null);
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
            XmlNode baseNode = GetBaseTypeNode(source, typeName);
            targetDocumentation = GetSimpleMemberDocumentation(source, baseNode);
        }
        else if (prefix is "P:" or "M:" or "F:")
        {
            XmlNode baseNode = GetBaseMemberNode(source, typeName, memberName);
            targetDocumentation = GetSimpleMemberDocumentation(source, baseNode);
        }

        if (targetDocumentation == null)
        {
            return null;
        }

        return new DocumentationReference(node, targetDocumentation);
    }

    private XmlNode GetBaseMemberNode(XDoc source, string typeName, string memberName)
    {
        throw new NotImplementedException();
    }

    private XmlNode GetBaseTypeNode(XDoc source, string typeName)
    {
        throw new NotImplementedException();
    }

    private MemberDocumentation? GetSimpleMemberDocumentation(XDoc source, XmlNode? node)
    {
        return new SimpleMemberDocumentation(source, node);
    }
}