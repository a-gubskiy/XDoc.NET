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
    /// Gets or initializes the code reference string that identifies the target element.
    /// </summary>
    public string? Cref { get; init; }
    
    /// <summary>
    /// Target member documentation
    /// </summary>
    public MemberDocumentation? Target { get; private init; }

    /// <summary>
    /// Default constructor
    /// </summary>
    /// <param name="requirementNode"></param>
    /// <param name="target"></param>
    /// <param name="cref"></param>
    public DocumentationReference(XmlNode requirementNode, MemberDocumentation? target, string? cref)
    {
        RequirementNode = requirementNode;
        Target = target;
        Cref = cref;
    }
}