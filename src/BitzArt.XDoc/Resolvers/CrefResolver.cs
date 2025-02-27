using System.Collections.Immutable;
using System.Xml;
using System.Xml.Linq;

namespace BitzArt.XDoc.Resolvers;

public class CrefResolver
{
    // private readonly XDoc _source;
    //
    // public CrefResolver(XDoc source)
    // {
    //     _source = source;
    // }

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
            .Select(o => o.Substring(2, o.Length - 2))
            .Distinct()
            .ToImmutableList();

        foreach (var r in refs)
        {
            if (string.IsNullOrWhiteSpace(r))
            {
                continue;
            }

            var type = GetType(r);
            var target = source.Get(type);
            
            var reference = new SeeMemberDocumentationReference
            {
                Target = target,
                RequirementNode = documentation.Node,
                ReferencedType = r
            };

            result.Add(reference);
        }

        return result;
    }
    
    private static Type GetType(string typeName)
    {
        var type = AppDomain.CurrentDomain
            .GetAssemblies()
            .Select(a => a.GetType(typeName, false))
            .FirstOrDefault(t => t != null); // What if we have multiple types with the same name and namespace?

        return type;
    }
}