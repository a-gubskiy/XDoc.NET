using System.Reflection;
using System.Xml;

namespace BitzArt.XDoc;

/// <summary>
/// Resolves documentation references from XML nodes.
/// </summary>
/// <remarks>
/// This interface is used to extract documentation references from XML documentation comments,
/// such as cref links and inheritdoc elements, to create structured representation of code references.
/// </remarks>
public interface IDocumentationReferenceResolver
{
    /// <summary>
    /// Extracts a documentation reference from the provided XML node.
    /// </summary>
    /// <param name="node">The XML node to extract the reference from.</param>
    /// <returns>
    /// A <see cref="DocumentationReference"/> object if a reference can be extracted;
    /// otherwise, <see langword="null"/>.
    /// </returns>
    DocumentationReference? GetReference(XmlNode node);
}

/// <summary>
/// Default implementation of <see cref="IDocumentationReferenceResolver"/> that extracts references from XML documentation nodes.
/// </summary>
public class DocumentationReferenceResolver : IDocumentationReferenceResolver
{
    private readonly XDoc _source;

    public DocumentationReferenceResolver(XDoc source)
    {
        _source = source;
    }

    /// <summary>
    /// Extracts a documentation reference from the provided XML node.
    /// </summary>
    /// <param name="node">The XML node to extract the reference from.</param>
    /// <returns>
    /// A <see cref="DocumentationReference"/> object if a reference can be extracted;
    /// otherwise, <see langword="null"/>.
    /// </returns>
    /// <exception cref="NotImplementedException">Thrown when the node type is not supported.</exception>
    public DocumentationReference? GetReference(XmlNode node)
    {
        var cref = node.Attributes?["cref"];

        if (cref != null)
        {
            return GetReference(node, cref);
        }

        if (node.Name == "inheritdoc")
        {
            return GetInheritReference(node);
        }

        return null;
    }

    /// <summary>
    /// Processes an inheritdoc XML node to extract documentation reference.
    /// </summary>
    /// <param name="node">The inheritdoc XML node.</param>
    /// <returns>A documentation reference or null if reference cannot be extracted.</returns>
    /// <exception cref="NotImplementedException">This method is not implemented yet.</exception>
    private DocumentationReference? GetInheritReference(XmlNode node)
    {
        var nodeParentNode = node.ParentNode;
        var attribute = nodeParentNode.Attributes["name"];

        var prefix = attribute.Value[..2] ?? "";
        var name = attribute?.Value.Substring(2, attribute.Value.Length - 2) ?? string.Empty;
        var (typeName, memberName) = GetTypeAndMember(name);

        var type = GetType(typeName);
        var baseType = type.BaseType;

        if (baseType == null)
        {
            return null;
        }

        if (prefix == "T:")
        {
            var targetDocumentation = _source.Get(baseType);

            if (targetDocumentation == null)
            {
                return null;
            }

            return new DocumentationReference(node, targetDocumentation);
        }

        if (prefix == "P:" || prefix == "M:" || prefix == "F:")
        {
            var baseMemberInfos = baseType.GetMember(memberName);

            if (baseMemberInfos.Length != 1)
            {
                throw new Exception("Can't select a single member.");
            }

            var baseMemberInfo = baseMemberInfos[0];
            
            var memberDocumentation = _source.Get(baseMemberInfo);

            if (memberDocumentation == null)
            {
                return null;
            }

            return new DocumentationReference(node, memberDocumentation);
        }

        return null;
    }

    /// <summary>
    /// Processes an XML node with a cref attribute to extract documentation reference.
    /// </summary>
    /// <param name="node">The XML node containing the reference.</param>
    /// <param name="attribute">The cref attribute containing the reference value.</param>
    /// <returns>A documentation reference or null if reference cannot be extracted.</returns>
    /// <exception cref="NotImplementedException">This method is not implemented yet.</exception>
    private DocumentationReference? GetReference(XmlNode node, XmlAttribute? attribute)
    {
        var targetDocumentation = GetTargetDocumentation(node, attribute);

        if (targetDocumentation == null)
        {
            return null;
        }

        // <see cref="P:MyCompany.Library.WeeklyMetrics.Progress" />

        return new DocumentationReference(node, targetDocumentation);
    }

    private MemberDocumentation? GetTargetDocumentation(XmlNode node, XmlAttribute? attribute)
    {
        if (attribute == null || attribute.Value.Length < 2)
        {
            return null;
        }

        var prefix = attribute.Value[..2] ?? "";
        var name = attribute?.Value.Substring(2, attribute.Value.Length - 2) ?? string.Empty;

        var targetDocumentation = prefix switch
        {
            "T:" => GetTypeReference(name),
            "P:" => GetPropertyReference(name),
            "M:" => GetMethodReference(name),
            "F:" => GetFieldReference(name),
            _ => throw new NotSupportedException()
        };

        return targetDocumentation;
    }

    private MemberDocumentation? GetFieldReference(string name)
    {
        var (typeName, memberName) = GetTypeAndMember(name);

        var type = GetType(typeName);
        var fieldInfo = type.GetField(memberName);
        var targetDocumentation = _source.Get(fieldInfo!);

        return targetDocumentation;
    }

    private MemberDocumentation? GetMethodReference(string name)
    {
        var (typeName, memberName) = GetTypeAndMember(name);

        var type = GetType(typeName);
        var methodInfo = type.GetMethod(memberName);
        var targetDocumentation = _source.Get(methodInfo!);

        return targetDocumentation;
    }

    private MemberDocumentation? GetPropertyReference(string name)
    {
        var (typeName, memberName) = GetTypeAndMember(name);

        var type = GetType(typeName);
        var propertyInfo = type.GetProperty(memberName);
        var targetDocumentation = _source.Get(propertyInfo!);

        return targetDocumentation;
    }

    private MemberDocumentation? GetTypeReference(string name)
    {
        var type = GetType(name);
        var targetDocumentation = _source.Get(type);

        return targetDocumentation;
    }

    private (string typeName, string memberName) GetTypeAndMember(string name)
    {
        var lastIndexOf = name.LastIndexOf('.');

        var typeName = name[..lastIndexOf];
        var memberName = name[(lastIndexOf + 1)..];

        return (typeName, memberName);
    }

    /// <summary>
    /// Resolves a Type object from its string name representation.
    /// </summary>
    /// <param name="name">The fully qualified name of the type to resolve.</param>
    /// <returns>The resolved Type object.</returns>
    /// <exception cref="TypeLoadException">Thrown when the specified type cannot be found.</exception>
    private Type GetType(string name)
    {
        // First try direct type resolution
        var type = Type.GetType(name);

        if (type != null)
        {
            return type;
        }

        // If direct lookup fails, search through all loaded assemblies
        var assemblies = AppDomain.CurrentDomain.GetAssemblies();

        foreach (var assembly in assemblies)
        {
            type = assembly.GetType(name);

            if (type != null)
            {
                return type;
            }
        }

        throw new TypeLoadException($"Could not find type '{name}'.");
    }
}