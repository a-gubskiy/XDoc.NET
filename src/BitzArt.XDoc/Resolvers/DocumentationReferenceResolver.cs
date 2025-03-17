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
        var attribute = node.ParentNode?.Attributes?["name"];
        var referenceName = attribute?.Value ?? string.Empty;

        if (string.IsNullOrWhiteSpace(referenceName) || referenceName.Length < 2)
        {
            return null;
        }

        var prefix = referenceName[..2];
        var name = referenceName.Substring(2, referenceName.Length - 2);

        var (typeName, memberName) = GetTypeAndMember(name);

        var type = GetType(typeName);
        var baseType = type.BaseType;

        if (baseType == null)
        {
            return null;
        }

        MemberDocumentation? targetDocumentation = null;

        if (prefix is "T:")
        {
            targetDocumentation = _source.Get(baseType);
        }
        else if (prefix is "P:" or "M:" or "F:")
        {
            targetDocumentation = GetMemberDocumentation(baseType, memberName);
        }

        if (targetDocumentation == null)
        {
            return null;
        }

        return new DocumentationReference(node, targetDocumentation);
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
        if (attribute == null || attribute.Value.Length < 2)
        {
            return null;
        }

        var prefix = attribute.Value[..2];
        var name = attribute.Value.Substring(2, attribute.Value.Length - 2) ?? string.Empty;

        var (typeName, memberName) = GetTypeAndMember(name);
        var type = GetType(typeName);

        MemberDocumentation? targetDocumentation = null;
        
        if (prefix is "T:")
        {
            targetDocumentation = _source.Get(type);
        }
        else if (prefix is "P:" or "M:" or "F:")
        {
            targetDocumentation = GetMemberDocumentation(type, memberName);
        }

        if (targetDocumentation == null)
        {
            return null;
        }

        // <see cref="P:MyCompany.Library.WeeklyMetrics.Progress" />

        return new DocumentationReference(node, targetDocumentation);
    }

    private MemberDocumentation? GetMemberDocumentation(Type type, string memberName)
    {
        var memberInfos = type.GetMember(memberName);

        if (memberInfos.Length != 1)
        {
            throw new Exception("Can't select a member info.");
        }

        var memberInfo = memberInfos.First();
        var memberDocumentation = _source.Get(memberInfo);

        return memberDocumentation;
    }

    /// <summary>
    /// Splits a fully qualified member name into its type name and member name components.
    /// </summary>
    /// <param name="name">The fully qualified name to split (e.g. "Namespace.Class.Method").</param>
    /// <returns>
    /// A tuple containing the type name (everything before the last period) and 
    /// the member name (everything after the last period).
    /// </returns>
    /// <example>
    /// For input "System.String.Length", returns ("System.String", "Length").
    /// </example>
    private static (string typeName, string memberName) GetTypeAndMember(string name)
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
    private static Type GetType(string name)
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