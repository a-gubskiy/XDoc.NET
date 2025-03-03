using System.Xml;

namespace BitzArt.XDoc;

public abstract class MemberDocumentationReference
{
    /// <summary>
    /// Actual XML node that caused this requirement
    /// </summary>
    public XmlNode RequirementNode { get; internal init; }
}

/// <summary>
/// Same as MemberDocumentationReference but this class means the XML node will be inheritdoc,
/// can later contain some code, for now can be just a class that does not bring anything specific with it
/// aside from what it inherits from MemberDocumentationReference.
/// </summary>
public class InheritanceMemberDocumentationReference : MemberDocumentationReference
{
    public Type TargetType { get; internal set; }

    // This class can then be used something like the following:
    // private void ExampleUsage(MemberDocumentationReference someReference)
    // {
    //     // Let's assume we are currently iterating through a list of MemberDocumentationReferences or something
    //
    //     // this gives you knowledge that this reference was caused by an <inheritdoc> node
    //     if (someReference is InheritanceMemberDocumentationReference inheritanceReference)
    //     {
    //         // do something specific to inheritance reference
    //     }
    // }
}

/// <summary>
/// Same as InheritanceMemberDocumentationReference but in this case the node is <see> instead of <inheritdoc>
/// </summary>
public class SeeMemberDocumentationReference : MemberDocumentationReference
{
    public string CrefValue { get; internal set; } = "";
}