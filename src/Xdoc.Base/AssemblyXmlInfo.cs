namespace Xdoc;

public record AssemblyXmlInfo
{
    public string Name { get; init; }
    public string Xml { get; init; }

    public AssemblyXmlInfo(string name, string xml)
    {
        Name = name;
        Xml = xml;
    }

    public PropertyXmlInfo Get(Type type, string propertyName)
    {
        throw new NotImplementedException();
    }

    public ClassXmlInfo Get(Type type)
    {
        throw new NotImplementedException();
    }
}