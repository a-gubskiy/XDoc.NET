using System.Xml;

namespace BitzArt.XDoc;

public abstract class MemberDocumentationReference
{
    /// <summary>
    /// Actual XML node that caused this requirement
    /// </summary>
    public XmlNode RequirementNode { get; private init; }

    /// <summary>
    /// Default constructor
    /// </summary>
    /// <param name="requirementNode"></param>
    protected MemberDocumentationReference(XmlNode requirementNode)
    {
        RequirementNode = requirementNode;
    }
}

/// <summary>
/// Same as MemberDocumentationReference but this class means the XML node will be inheritdoc,
/// can later contain some code, for now can be just a class that does not bring anything specific with it
/// aside from what it inherits from MemberDocumentationReference.
/// </summary>
public class InheritanceMemberDocumentationReference : MemberDocumentationReference
{
    /// <summary>
    /// Default constructor
    /// </summary>
    /// <param name="requirementNode"></param>
    /// <param name="targetType"></param>
    public InheritanceMemberDocumentationReference(XmlNode requirementNode, Type targetType)
        : base(requirementNode)
    {
        TargetType = targetType;
    }

    /// <summary>
    /// The type that this node is inheriting from
    /// </summary>
    public Type TargetType { get; private set; }
}

/// <summary>
/// Same as InheritanceMemberDocumentationReference but in this case the node is <see> instead of <inheritdoc>
/// </summary>
public class SeeMemberDocumentationReference : MemberDocumentationReference
{
    /// <summary>
    /// Default constructor
    /// </summary>
    /// <param name="requirementNode"></param>
    /// <param name="crefValue"></param>
    public SeeMemberDocumentationReference(XmlNode requirementNode, string crefValue) : base(requirementNode)
    {
        CrefValue = crefValue;
    }

    /// <summary>
    /// The cref value of the <see> node
    /// </summary>
    public string CrefValue { get; private set; }
}