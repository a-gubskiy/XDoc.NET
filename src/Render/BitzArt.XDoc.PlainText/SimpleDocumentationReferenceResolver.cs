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
        
        var baseType = "";
        
        MemberDocumentation? targetDocumentation = null;

        if (prefix is "T:")
        {
            targetDocumentation = GetSimpleMemberDocumentation(source, node, baseType);
        }
        else if (prefix is "P:" or "M:" or "F:")
        {
            targetDocumentation = GetSimpleMemberDocumentation(source, node, baseType, memberName);
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

        // var type = GetType(typeName);

        MemberDocumentation? targetDocumentation = null;

        if (prefix is "T:")
        {
            targetDocumentation = GetSimpleMemberDocumentation(source, node, typeName);
        }
        else if (prefix is "P:" or "M:" or "F:")
        {
            targetDocumentation = GetSimpleMemberDocumentation(source, node, typeName, memberName);
        }

        if (targetDocumentation == null)
        {
            return null;
        }

        return new DocumentationReference(node, targetDocumentation);
    }
    
    private MemberDocumentation? GetSimpleMemberDocumentation(XDoc source, XmlNode node, string baseType, string memberName)
    {
        return new SimpleMemberDocumentation(source, node);
    }

    private MemberDocumentation? GetSimpleMemberDocumentation(XDoc source, XmlNode node, string baseType)
    {
        return new SimpleMemberDocumentation(source, node);
    }
    
    public class SimpleMemberDocumentation : MemberDocumentation
    {
        public SimpleMemberDocumentation(XDoc source, XmlNode? node)
            : base(source, node)
        {
        }
    }
}