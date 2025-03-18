using System.Xml;

namespace BitzArt.XDoc;

/// <summary>
/// Represents a simple documentation reference that may reference other code elements 
/// or inherit documentation from base classes.
/// </summary>
public class SimpleDocumentationReference : DocumentationReference
{
    /// <summary>
    /// Gets or initializes the code reference string that identifies the target element.
    /// </summary>
    public string Cref { get; init; }
    
    /// <summary>
    /// Gets or initializes a value indicating whether this documentation inherits from another element.
    /// </summary>
    public bool IsInheritDoc { get; init; }

    /// <summary>
    /// Initializes a new instance of the <see cref="SimpleDocumentationReference"/> class.
    /// </summary>
    /// <param name="requirementNode">The XML node containing the documentation requirement.</param>
    /// <param name="cref">The code reference string identifying the target element.</param>
    /// <param name="isInheritDoc">A value indicating whether this documentation inherits from another element.</param>
    public SimpleDocumentationReference(XmlNode requirementNode, string cref, bool isInheritDoc)
        : base(requirementNode, null)
    {
        Cref = cref;
        IsInheritDoc = isInheritDoc;
    }
}