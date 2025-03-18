using System.Xml;

namespace BitzArt.XDoc;

/// <summary>
/// Represents a class for documentation references.
/// </summary>
public class DocumentationReference
{
    /// <summary>
    /// Actual XML node that caused this requirement
    /// </summary>
    public XmlNode RequirementNode { get; private init; }
    
    /// <summary>
    /// Target member documentation
    /// </summary>
    public MemberDocumentation? Target { get; private init; }

    /// <summary>
    /// Default constructor
    /// </summary>
    /// <param name="requirementNode"></param>
    /// <param name="target"></param>
    public DocumentationReference(XmlNode requirementNode, MemberDocumentation? target)
    {
        RequirementNode = requirementNode;
        Target = target;
    }
}