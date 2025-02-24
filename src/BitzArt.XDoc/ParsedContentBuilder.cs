namespace BitzArt.XDoc;

public class ParsedContentBuilder
{
    public ParsedContent Build(TypeDocumentation typeDocumentation)
    {
        var xmlNode = typeDocumentation.Node;
        var xDoc = typeDocumentation.Source;
        
        throw new NotImplementedException();
    }
    
    public ParsedContent Build<T>(MemberDocumentation<T>  memberDocumentation) where T : class
    {
        var xmlNode = memberDocumentation.Node;
        var xDoc = memberDocumentation.Source;

        throw new NotImplementedException();
    }
}