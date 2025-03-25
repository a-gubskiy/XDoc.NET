using System.Xml;

namespace BitzArt.XDoc;

/// <summary>
/// All documentation classes should inherit from this class.
/// This class contains code applicable to any member documentation (both Type and MemberInfo).
/// </summary>
public abstract class DocumentationElement : IDocumentationElement
{
    private readonly Dictionary<XmlNode, DocumentationReference> _references = [];

    /// <summary>
    /// XML documentation node.
    /// </summary>
    public XmlNode? Node { get; private init; }

    /// <summary>
    /// Source <see cref="XDoc"/> instance used to generate this <see cref="DocumentationElement"/>.
    /// </summary>
    public XDoc Source { get; private init; }

    private bool IsMyNode(XmlNode node)
    {
        while (node.ParentNode != null)
        {
            if (node.ParentNode == Node)
            {
                return true;
            }

            node = node.ParentNode;
        }

        return false;
    }

    public DocumentationReference? GetReference(XmlNode requirementNode)
    {
        // <summary> This is my <see cref="BitzArt.XDoc.Models.TypeDocumentation"/>. </summary>


        // // <inheritdoc cref="Name" /> 
        // // <inheritdoc /> 

        if (_references.TryGetValue(requirementNode, out var result))
        {
            return result;
        }

        if (!IsMyNode(requirementNode))
        {
            throw new InvalidOperationException("The provided node is not defined in this documentation.");
        }

        var documentationReference = Source.ReferenceResolver.GetReference(Source, requirementNode);

        _references[requirementNode] = documentationReference;

        return documentationReference;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="DocumentationElement"/> class.
    /// </summary>
    /// <param name="source">The source of the documentation.</param>
    /// <param name="node">The XML node that contains the documentation.</param>
    protected DocumentationElement(XDoc source, XmlNode? node)
    {
        Source = source;
        Node = node;
    }
}