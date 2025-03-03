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
            var type = GetType(referencedType);

            var reference = new SeeMemberDocumentationReference
            {
                RequirementNode = documentation.Node,
            };

            result.Add(reference);
        }

        return result;
    }


    private static Type? GetType(string? typeName)
    {
        if (string.IsNullOrWhiteSpace(typeName))
        {
            return null;
        }
        
        var type = AppDomain.CurrentDomain
            .GetAssemblies()
            .Select(a => a.GetType(typeName, false))
            .FirstOrDefault(t => t != null); // What if we have multiple types with the same name and namespace?

        return type;
    }
}