using System.Reflection;
using System.Xml;
using System.Xml.Linq;

namespace BitzArt.XDoc;

public class ParsedContentBuilder
{
    public ParsedContent Build(TypeDocumentation typeDocumentation)
    {
        var xmlNode = typeDocumentation.Node ?? new XmlDocument();
        var xDoc = typeDocumentation.Source;
        var type = typeDocumentation.Type;

        var parent = GetParent(xmlNode, xDoc, type);

        var references = GetReferences(xmlNode, xDoc);

        var parsedContent = new ParsedContent
        {
            Parent = parent,
            References = references,
            OriginalNode = xmlNode,
            Type = type
        };

        return parsedContent;
    }

    private IReadOnlyCollection<ParsedContent> GetReferences(XmlNode xmlNode, XDoc xDoc)
    {
        var doc = XDocument.Parse(xmlNode.InnerXml);

        var refs = doc.Descendants("see")
            .Select(e => e.Attribute("cref")?.Value)
            .Where(value => !string.IsNullOrWhiteSpace(value))
            .Select(o => o!)
            .Distinct()
            .ToList();

        //xDoc.Get()

        var references = new List<ParsedContent>();

        foreach (var r in refs)
        {
            var typeName = r.Substring(2, r.Length - 2);
            var type = Type.GetType(typeName);

            if (type == null)
            {
                continue;
            }

            var typeDocumentation = xDoc.Get(type);

            references.Add(new ParsedContent
            {
                Type = type,
                OriginalNode = typeDocumentation.Node,
                References = GetReferences(typeDocumentation.Node, xDoc),
                Parent = GetParent(typeDocumentation.Node, xDoc, type)
            });
        }

        return references;
    }

    private ParsedContent? GetParent(XmlNode xmlNode, XDoc xDoc, Type type)
    {
        if (xmlNode?.FirstChild?.Name == "inheritdoc")
        {
            var parentTypeDocumentation = xDoc.Get(type.BaseType);

            var parent = GetParent(parentTypeDocumentation.Node, xDoc, type.BaseType);

            return parent;
        }
        // <member name="P:TestAssembly.B.Dog.Field1">
        //     <inheritdoc/>
        //     </member>

        return null;
    }

    public ParsedContent Build<T>(MemberDocumentation<T> memberDocumentation) where T : class
    {
        var xmlNode = memberDocumentation.Node;
        var xDoc = memberDocumentation.Source;
        var type = memberDocumentation.DeclaringType;

        var parent = GetParent(xmlNode, xDoc, type);

        var references = GetReferences(xmlNode, xDoc);

        var parsedContent = new ParsedContent
        {
            Parent = parent,
            References = references,
            OriginalNode = xmlNode,
            Type = type
        };

        return parsedContent;
    }
}