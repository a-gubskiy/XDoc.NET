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

        var source = documentation.Source;

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
            var target = GetTypeDocumentation(type, source);

            var reference = new SeeMemberDocumentationReference
            {
                Target = target,
                RequirementNode = documentation.Node,
                ReferencedType = referencedType
            };

            result.Add(reference);
        }

        return result;
    }

    private static TypeDocumentation? GetTypeDocumentation(Type? type, XDoc source)
    {
        try
        {
            var target = type is null ? null : source.Get(type);
            return target;
        }
        catch (XDocException ex)
        {
            //We're trying to get documentation for a type that is not documented
            return null;
        }
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