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