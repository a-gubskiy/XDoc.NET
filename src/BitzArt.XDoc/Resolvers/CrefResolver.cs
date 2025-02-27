using System.Collections.Immutable;
using System.Xml;
using System.Xml.Linq;

namespace BitzArt.XDoc.Resolvers;

public class CrefResolver
{
    private readonly XDoc _source;

    public CrefResolver(XDoc source)
    {
        _source = source;
    }
    
    internal static IReadOnlyCollection<MemberDocumentationReference> Resolve(MemberDocumentation documentation)
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
            .Select(o => o.Substring(2, o.Length - 2))
            .Distinct()
            .ToImmutableList();

        foreach (var r in refs)
        {
            
            //var cref = crefNode.InnerText;

            if (string.IsNullOrWhiteSpace(r))
            {
                continue;
            }

            throw new NotImplementedException();

            // _source.Get()

            // result.Add(new MemberDocumentationReference
            // {
            //     Target = ,
            //     RequirementNode = 
            // });
        }

        return result;
    }
}