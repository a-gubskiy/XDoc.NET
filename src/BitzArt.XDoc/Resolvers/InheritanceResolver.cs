using System.Collections.Immutable;

namespace BitzArt.XDoc.Resolvers;

public class InheritanceResolver
{
    internal static InheritanceMemberDocumentationReference? Resolve(MemberDocumentation documentation)
    {
        if (documentation.Node == null || string.IsNullOrWhiteSpace(documentation.Node.InnerXml))
        {
            return null;
        }
        
        throw new NotImplementedException();
    }
}