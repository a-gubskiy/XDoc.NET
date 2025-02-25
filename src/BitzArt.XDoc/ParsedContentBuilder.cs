using System.Collections.Immutable;
using System.Xml;
using System.Xml.Linq;

namespace BitzArt.XDoc;

/// <summary>
/// Builds parsed content objects by processing XML documentation nodes
/// and resolving their references and inheritance hierarchy.
/// </summary>
/// <remarks>
/// This class is responsible for:
/// - Creating ParsedContent instances from TypeDocumentation and MemberDocumentation
/// - Resolving XML documentation inheritance chains
/// - Processing XML documentation references
/// </remarks>
public class ParsedContentBuilder
{
    /// <summary>
    /// Builds a ParsedContent object from <see cref="TypeDocumentation"/>  by processing its XML documentation and resolving references.
    /// </summary>
    /// <param name="typeDocumentation">The type documentation containing XML nodes and type information.</param>
    /// <returns>A ParsedContent object containing resolved documentation, references and inheritance chain.</returns>
    public ParsedContent Build(TypeDocumentation typeDocumentation) =>
        GetParsedContent(typeDocumentation.Node, typeDocumentation.Source, typeDocumentation.Type);

    /// <summary>
    /// Builds a ParsedContent object from <see cref="MemberDocumentation{T}"/> by processing its XML documentation and resolving references.
    /// </summary>
    /// <param name="memberDocumentation">The member documentation containing XML nodes and member information.</param>
    /// <returns>A <see cref="ParsedContent"/> object containing resolved documentation, references and inheritance chain.</returns>
    /// <typeparam name="T">The type of the member being documented.</typeparam>
    public ParsedContent Build<T>(MemberDocumentation<T> memberDocumentation) where T : class =>
        GetParsedContent(memberDocumentation.Node, memberDocumentation.Source, memberDocumentation.DeclaringType);

    private ParsedContent GetParsedContent(XmlNode? xmlNode, XDoc xDoc, Type type)
    {
        var parent = GetParent(xmlNode, xDoc, type);
        var references = GetReferences(xmlNode, xDoc);

        return new ParsedContent
        {
            Parent = parent,
            References = references,
            Xml = xmlNode,
            Type = type
        };
    }

    private IReadOnlyCollection<ParsedContent> GetReferences(XmlNode? xmlNode, XDoc xDoc)
    {
        if (xmlNode == null || string.IsNullOrWhiteSpace(xmlNode.InnerXml))
        {
            return ImmutableList<ParsedContent>.Empty;
        }

        var doc = XDocument.Parse(xmlNode.InnerXml);

        var refs = doc.Descendants("see")
            .Select(e => e.Attribute("cref")?.Value)
            .Where(value => !string.IsNullOrWhiteSpace(value))
            .Select(o => o!)
            .Distinct()
            .ToList();

        var references = new List<ParsedContent>();

        foreach (var r in refs)
        {
            var typeName = r.Substring(2, r.Length - 2);
            var type = GetTypeInfo(typeName);

            if (type == null)
            {
                continue;
            }

            var typeDocumentation = xDoc.Get(type);

            references.Add(new ParsedContent
            {
                Type = type,
                Xml = typeDocumentation?.Node,
                References = GetReferences(typeDocumentation?.Node, xDoc),
                Parent = GetParent(typeDocumentation?.Node, xDoc, type)
            });
        }

        return references;
    }

    private Type? GetTypeInfo(string typeName)
    {
        var type = AppDomain.CurrentDomain
            .GetAssemblies()
            .Select(a => a.GetType(typeName, false))
            .FirstOrDefault(t => t != null); // What if we have multiple types with the same name and namespace?

        return type;
    }

    private ParsedContent? GetParent(XmlNode? xmlNode, XDoc xDoc, Type type)
    {
        if (xmlNode?.FirstChild?.Name != "inheritdoc" || type.BaseType == null)
        {
            return null;
        }

        var parentTypeDocumentation = xDoc.Get(type.BaseType);

        var parent = GetParent(parentTypeDocumentation?.Node, xDoc, type.BaseType);

        return parent;
    }
}