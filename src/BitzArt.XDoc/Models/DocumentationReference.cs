// using System.Xml;
//
// namespace BitzArt.XDoc;
//
// /// <summary>
// /// Represents a class for documentation references.
// /// </summary>
// public class DocumentationReference
// {
//     /// <summary>
//     /// Actual XML node that caused this requirement
//     /// </summary>
//     public XmlNode RequirementNode { get; private init; }
//
//     // /// <summary>
//     // /// The code reference that identifies the target element.
//     // /// </summary>
//     // public Cref? Cref { get; init; }
//
//     // /// <summary>
//     // /// Target member documentation
//     // /// </summary>
//     // public DocumentationElement? Target { get; private init; }
//
//     /// <summary>
//     /// Default constructor
//     /// </summary>
//     /// <param name="requirementNode"></param>
//     /// <param name="target"></param>
//     /// <param name="crefValue"></param>
//     public DocumentationReference(XmlNode requirementNode, DocumentationElement? target)
//         // : this(requirementNode, target, Cref.TryCreate(crefValue, out var cref) ? cref : null)
//         : this(requirementNode, target)
//     {
//     }
//
//
//     // public DocumentationReference(XmlNode requirementNode, DocumentationElement? target, Cref? cref)
//     public DocumentationReference(XmlNode requirementNode, DocumentationElement? target)
//     {
//         Target = target;
//         RequirementNode = requirementNode;
//         // Cref = cref;
//     }
// }