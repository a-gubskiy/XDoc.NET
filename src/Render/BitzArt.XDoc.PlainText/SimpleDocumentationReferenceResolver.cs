using System.Xml;

namespace BitzArt.XDoc;

/// <inheritdoc />
public class SimpleDocumentationReferenceResolver : DocumentationReferenceResolver
{
    /// <inheritdoc />
    public override DocumentationReference? GetReference(XDoc source, XmlNode node)
    {
        var cref = node.Attributes?["cref"]?.Value ?? string.Empty;

        var isCref = !string.IsNullOrWhiteSpace(cref);
        var isInheritDoc = node.Name == "inheritdoc";
        var isSee = node.Name == "see";

        if (!isInheritDoc && !isCref && !isSee)
        {
            return null;
        }

        return new DocumentationReference(node, null, cref);
    }
}