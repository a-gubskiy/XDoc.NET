using System.Collections.Immutable;

namespace Xdoc;

public record ClassXmlInfo
{
    public AssemblyXmlInfo? Parent { get; init; }

    public IReadOnlyCollection<PropertyXmlInfo> Properties { get; init; }

    public string Name { get; init; }
    
    public ImmutableList<MethodXmlInfo> Methods { get; set; }

    public ClassXmlInfo()
    {
        Properties = ImmutableList<PropertyXmlInfo>.Empty;
    }

    
    public PropertyXmlInfo? Get(string propertyName)
    {
        var propertyXmlInfo = Properties.FirstOrDefault(p => p.Name == propertyName);
        
        return propertyXmlInfo;
    }
}