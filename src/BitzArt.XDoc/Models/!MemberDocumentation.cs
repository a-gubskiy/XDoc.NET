using System.Xml;

namespace BitzArt.XDoc;

/// <summary>
/// All documentation classes should inherit from this class.
/// This class contains code applicable to any member documentation (both Type and MemberInfo).
/// </summary>
public abstract class MemberDocumentation
{
    private readonly Dictionary<XmlNode, DocumentationReference?> _references = [];
    
    // Documentation of a code member:
    // - Type as a member of an Assembly;
    // - MemberInfo as a member of Type.

    /// <summary>
    /// XML node that contains the documentation.
    /// </summary>
    public XmlNode? Node { get; private init; }

    internal XDoc Source { get; private init; }

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
    
    public DocumentationReference? GetReference(XmlNode node)
    {
        // // <dsffsf cref="BitzArt.XDoc.Models.TypeDocumentation" /> 
        // // <inheritdoc cref="Name" /> 
        // // <inheritdoc /> 
        
        if (_references.TryGetValue(node, out var result))
        {
            return result;
        }

        if (!IsMyNode(node))
        {
            throw new InvalidOperationException("The provided node is not defined in this documentation.");
        }

        var documentationReference = Source.ReferenceResolver.GetReference(node);
        
        _references[node] = documentationReference;
        
        return documentationReference;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="MemberDocumentation"/> class.
    /// </summary>
    /// <param name="source">The source of the documentation.</param>
    /// <param name="node">The XML node that contains the documentation.</param>
    protected MemberDocumentation(XDoc source, XmlNode? node)
    {
        Source = source;
        Node = node;
    }
}