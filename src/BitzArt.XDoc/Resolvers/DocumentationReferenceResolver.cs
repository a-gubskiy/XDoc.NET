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
    /// <param name="source"></param>
    /// <param name="node">The XML node to extract the reference from.</param>
    /// <returns>
    /// A <see cref="DocumentationReference"/> object if a reference can be extracted;
    /// otherwise, <see langword="null"/>.
    /// </returns>
    DocumentationReference? GetReference(XDoc source, XmlNode node);
}

/// <summary>
/// Default implementation of <see cref="IDocumentationReferenceResolver"/> that extracts references from XML documentation nodes.
/// </summary>
public class DocumentationReferenceResolver : IDocumentationReferenceResolver
{
    /// <summary>
    /// Extracts a documentation reference from the provided XML node.
    /// </summary>
    /// <param name="source"></param>
    /// <param name="node">The XML node to extract the reference from.</param>
    /// <returns>
    /// A <see cref="DocumentationReference"/> object if a reference can be extracted;
    /// otherwise, <see langword="null"/>.
    /// </returns>
    /// <exception cref="NotImplementedException">Thrown when the node type is not supported.</exception>
    public DocumentationReference? GetReference(XDoc source, XmlNode node)
    {
        var cref = node.Attributes?["cref"];

        if (cref != null)
        {
            return GetCrefReference(source, node, cref);
        }

        if (node.Name == "inheritdoc")
        {
            return GetInheritReference(source, node);
        }

        return null;
    }

    /// <summary>
    /// Processes an inheritdoc XML node to extract documentation reference.
    /// </summary>
    /// <param name="source"></param>
    /// <param name="node">The inheritdoc XML node.</param>
    /// <returns>A documentation reference or null if reference cannot be extracted.</returns>
    /// <exception cref="NotImplementedException">This method is not implemented yet.</exception>
    private DocumentationReference? GetInheritReference(XDoc source, XmlNode node)
    {
        var attribute = node.ParentNode?.Attributes?["name"];

        //P:TestAssembly.B.Dog.Color
        var referenceName = attribute?.Value ?? string.Empty;
        var (prefix, typeName, memberName) = GetTypeAndMember(referenceName);

        var type = GetType(typeName);
        var baseType = type.BaseType;

        if (baseType == null)
        {
            return null;
        }

        MemberDocumentation? targetDocumentation = null;

        if (prefix is "T:")
        {
            targetDocumentation = source.Get(baseType);
        }
        else if (prefix is "P:" or "M:" or "F:")
        {
            targetDocumentation = GetMemberDocumentation(source, baseType, memberName);
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
    /// <param name="source"></param>
    /// <param name="node">The XML node containing the reference.</param>
    /// <param name="attribute">The cref attribute containing the reference value.</param>
    /// <returns>A documentation reference or null if reference cannot be extracted.</returns>
    /// <exception cref="NotImplementedException">This method is not implemented yet.</exception>
    private DocumentationReference? GetCrefReference(XDoc source, XmlNode node, XmlAttribute? attribute)
    {
        // P:TestAssembly.B.Dog.Name
        var referenceName = attribute?.Value ?? string.Empty;
        var (prefix, typeName, memberName) = GetTypeAndMember(referenceName);

        var type = GetType(typeName);

        MemberDocumentation? targetDocumentation = null;

        if (prefix is "T:")
        {
            targetDocumentation = source.Get(type);
        }
        else if (prefix is "P:" or "M:" or "F:")
        {
            targetDocumentation = GetMemberDocumentation(source, type, memberName);
        }

        if (targetDocumentation == null)
        {
            return null;
        }

        return new DocumentationReference(node, targetDocumentation);
    }

    private MemberDocumentation? GetMemberDocumentation(XDoc source, Type type, string memberName)
    {
        var memberInfos = type.GetMember(memberName);

        if (memberInfos.Length != 1)
        {
            throw new Exception("Can't select a member info.");
        }

        var memberInfo = memberInfos.First();
        var memberDocumentation = source.Get(memberInfo);

        return memberDocumentation;
    }
    
    /// <summary>
    /// Parses a fully qualified XML documentation reference string into its constituent parts.
    /// </summary>
    /// <param name="value">The XML documentation reference string to parse (e.g., "T:Namespace.TypeName" or "P:Namespace.TypeName.PropertyName").</param>
    /// <returns>
    /// A tuple containing:
    /// - prefix: The reference type prefix (e.g., "T:", "P:", "M:", "F:")
    /// - typeName: The fully qualified type name
    /// - memberName: The member name (empty for type references)
    /// </returns>
    /// <exception cref="Exception">Thrown when the reference format is not recognized.</exception>
    /// <remarks>
    /// Handles these reference formats:
    /// - "T:Namespace.TypeName" for types
    /// - "P:Namespace.TypeName.PropertyName" for properties 
    /// - "M:Namespace.TypeName.MethodName" for methods
    /// - "F:Namespace.TypeName.FieldName" for fields
    /// </remarks>
    private static (string prefix, string typeName, string memberName) GetTypeAndMember(string value)
    {
        if (string.IsNullOrWhiteSpace(value) || value.Length < 2)
        {
            return (string.Empty, string.Empty, string.Empty);
        }

        var prefix = value[..2];

        if (prefix is "T:")
        {
            var typeName = value.Substring(2, value.Length - 2);

            return (prefix, typeName, string.Empty);
        }
        else if (prefix is "P:" or "M:" or "F:")
        {
            var lastIndexOf = value.LastIndexOf('.');

            var typeName = value.Substring(2, lastIndexOf - 2);
            var memberName = value[(lastIndexOf + 1)..];

            return (prefix, typeName, memberName);
        }

        throw new Exception("Can't select a member info.");
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