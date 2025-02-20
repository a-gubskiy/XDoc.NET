using System.Collections.Frozen;
using System.Xml;

namespace Xdoc.Models;

public record ClassXmlInfo : ISummarized
{
    private readonly Type _type;
    
    public IReadOnlyDictionary<string, PropertyXmlInfo> Properties { get; init; }

    public string Name => _type.FullName!;

    public AssemblyXmlInfo Assembly { get; init; }

    public XmlSummary Summary { get; init; }

    // public ClassXmlInfo? Parent => Assembly.DocumentStore.GetClassInfo(_type.BaseType);

    internal ClassXmlInfo(Type type, AssemblyXmlInfo assembly, IReadOnlyDictionary<string, XmlNode> documentation)
    {
        _type = type;

        Assembly = assembly;
        Summary = CreateSummary(type, documentation);
        Properties = CreateProperties(documentation);
    }

    private static XmlSummary CreateSummary(Type type, IReadOnlyDictionary<string, XmlNode> documentation)
    {
        var typeNameKey = $"T:{type.FullName}";
        var xmlNode = documentation.GetValueOrDefault(typeNameKey);
        
        return new XmlSummary(xmlNode);
    }

    private IReadOnlyDictionary<string, PropertyXmlInfo> CreateProperties(IReadOnlyDictionary<string, XmlNode> documentation)
    {
        var typeNamePrefix = $"P:{_type.FullName}.";
        
        var propertyKeys = documentation.Keys
            .Where(k => k.StartsWith(typeNamePrefix))
            .ToFrozenSet();

        var result = new Dictionary<string, PropertyXmlInfo>();

        foreach (var propertyKey in propertyKeys)
        {
            var propertyName = propertyKey[(propertyKey.LastIndexOf('.') + 1)..];

            var node = documentation[propertyKey];
            var propertyXmlInfo = new PropertyXmlInfo(propertyName, this, node);

            result.Add(propertyXmlInfo.Name, propertyXmlInfo);
        }

        return result.ToFrozenDictionary();
    }

    /// <summary>
    /// Get information about a property.
    /// </summary>
    /// <param name="propertyName"></param>
    /// <returns></returns>
    public PropertyXmlInfo? GetPropertyInfo(string propertyName)
    {
        return Properties.GetValueOrDefault(propertyName);
    }

    public override string ToString() => Name;
}