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
    /// The code reference that identifies the target element.
    /// </summary>
    public Cref? Cref { get; init; }

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
        Target = target;
        RequirementNode = requirementNode;
        Cref = string.IsNullOrWhiteSpace(cref) ? null : new Cref(cref);
    }
}