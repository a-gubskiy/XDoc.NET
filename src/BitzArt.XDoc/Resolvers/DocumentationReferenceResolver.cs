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

        throw new NotImplementedException();
    }

    /// <summary>
    /// Processes an inheritdoc XML node to extract documentation reference.
    /// </summary>
    /// <param name="node">The inheritdoc XML node.</param>
    /// <returns>A documentation reference or null if reference cannot be extracted.</returns>
    /// <exception cref="NotImplementedException">This method is not implemented yet.</exception>
    private DocumentationReference? GetInheritReference(XmlNode node)
    {
        throw new NotImplementedException();
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
        throw new NotImplementedException();
    }
}