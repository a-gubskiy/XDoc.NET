using System.Xml;

namespace BitzArt.XDoc;

public abstract class MemberDocumentationReference
{
    /// <summary>
    /// Actual XML node that caused this requirement
    /// </summary>
    public XmlNode RequirementNode { get; internal init; }

    /// <summary>
    /// Requirement target (resolved via cref / inheritdoc)
    /// </summary>
    public MemberDocumentation? Target { get; internal init; }

    public string? ReferencedType { get; internal init; }
}