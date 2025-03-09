using System.Collections.Immutable;
using System.Xml;
using System.Xml.Linq;

namespace BitzArt.XDoc.Resolvers;

internal static class CrefResolver
{
    public static IReadOnlyCollection<MemberDocumentationReference> Resolve(MemberDocumentation documentation)
    {
        if (documentation.Node == null || string.IsNullOrWhiteSpace(documentation.Node.InnerXml))
        {
            return ImmutableList<MemberDocumentationReference>.Empty;
        }

        var result = new List<MemberDocumentationReference>();

        var doc = XDocument.Parse(documentation.Node.InnerXml);

        var refs = doc.Descendants("see")
            .Select(e => e.Attribute("cref")?.Value)
            .Where(value => !string.IsNullOrWhiteSpace(value))
            .Select(o => o!.Substring(2, o.Length - 2))
            .Distinct()
            .ToImmutableList();

        foreach (var referencedType in refs)
        {
            var reference = new SeeMemberDocumentationReference(documentation.Node, referencedType);

            result.Add(reference);
        }

        return result;
    }
}