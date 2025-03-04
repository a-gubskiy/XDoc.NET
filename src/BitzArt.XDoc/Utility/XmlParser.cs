using System.Reflection;
using System.Xml;

namespace BitzArt.XDoc;

internal class XmlParser
{
    private readonly IXDoc _source;
    private readonly Assembly _assembly;
    private readonly XmlDocument _xml;

    private readonly Dictionary<Type, TypeDocumentation> _results;

    internal Dictionary<Type, TypeDocumentation> Results => _results;

    public static Dictionary<Type, TypeDocumentation> Parse(IXDoc source, Assembly assembly, XmlDocument xml)
    {
        var parser = new XmlParser(source, assembly, xml);
        parser.Parse();

        return parser.Results;
    }

    internal XmlParser(IXDoc source, Assembly assembly, XmlDocument xml)
    {
        _source = source;
        _assembly = assembly;
        _xml = xml;

        _results = [];
    }

    internal Dictionary<Type, TypeDocumentation> Parse()
    {
        var nodeList = _xml.SelectNodes("/doc/members/member")
                       ?? throw new InvalidOperationException("No documentation found in the XML file.");

        foreach (XmlNode node in nodeList) Parse(node);

        return _results;
    }

    private void Parse(XmlNode node)
    {
        if (node.Attributes is null || node.Attributes.Count == 0)
            throw new InvalidOperationException("Invalid XML node.");

        var name = node.Attributes["name"]?.Value;

        if (name == null)
        {
            throw new InvalidOperationException($"No 'name' attribute found in XML node '{node.Value}'.");
        }

        switch (name[0])
        {
            case 'T': ParseTypeNode(node, name[2..]); break;
            case 'P': ParsePropertyNode(node, name[2..]); break;
            case 'F': ParseFieldNode(node, name[2..]); break;
            case 'M': ParseMethodNode(node, name[2..]); break;
            default: break;
        }
    }

    private TypeDocumentation ParseTypeNode(XmlNode node, string name)
    {
        var type = _assembly.GetType(name);
        if (type == null)
        {
            throw new InvalidOperationException($"Type '{name}' not found.");
        }

        // We could handle this case by finding and updating the existing object,
        // but I don't see a reason why this would be necessary.
        if (_results.ContainsKey(type))
        {
            throw new InvalidOperationException("Invalid XML. Type nodes should always go first in the XML file");
        }

        var typeDocumentation = new TypeDocumentation(_source, type, node);

        _results[type] = typeDocumentation;

        return typeDocumentation;
    }

    private PropertyDocumentation ParsePropertyNode(XmlNode node, string name)
    {
        var (type, memberName) = ResolveTypeAndMemberName(name);

        var propertyInfo = type.GetProperty(memberName)
            ?? throw new InvalidOperationException($"Property '{memberName}' not found in type '{type.Name}'.");

        var typeDocumentation = ResolveOwnerType(type);

        var propertyDocumentation = new PropertyDocumentation(_source, typeDocumentation, propertyInfo, node);

        typeDocumentation.AddMemberData(propertyInfo, propertyDocumentation);

        return propertyDocumentation;
    }

    private FieldDocumentation ParseFieldNode(XmlNode node, string name)
    {
        var (type, memberName) = ResolveTypeAndMemberName(name);
        
        var fieldInfo = type.GetField(memberName)
                           ?? throw new InvalidOperationException($"Field '{memberName}' not found in type '{type.Name}'.");
        
        if (fieldInfo is null)
        {
            throw new InvalidOperationException($"Field '{memberName}' not found in type '{type.Name}'.");
        }

        var typeDocumentation = ResolveOwnerType(type);

        var fieldDocumentation = new FieldDocumentation(_source, typeDocumentation, fieldInfo, node);

        typeDocumentation.AddMemberData(fieldInfo, fieldDocumentation);

        return fieldDocumentation;
    }

    private MethodDocumentation ParseMethodNode(XmlNode node, string name)
    {
        var (type, memberName) = ResolveTypeAndMemberName(name);
        
        var methodInfo = type.GetMethod(memberName)
                           ?? throw new InvalidOperationException($"Method '{memberName}' not found in type '{type.Name}'.");

        if (methodInfo is null)
        {
            throw new InvalidOperationException($"Method '{memberName}' not found in type '{type.Name}'.");
        }

        var typeDocumentation = ResolveOwnerType(type);

        var methodDocumentation = new MethodDocumentation(_source, typeDocumentation, methodInfo, node);

        typeDocumentation.AddMemberData(methodInfo, methodDocumentation);

        return methodDocumentation;
    }
    
    private (Type type, string memberName) ResolveTypeAndMemberName(string name)
    {
        var index = name.LastIndexOf('.');

        if (index == -1)
        {
            throw new InvalidOperationException("Encountered invalid XML node.");
        }
        
        var (typeName, memberName) = (name[..index], name[(index + 1)..]);
        
        var type = _assembly.GetType(typeName)
                   ?? throw new InvalidOperationException($"Type '{typeName}' not found.");

        return (type, memberName);
    }

    private TypeDocumentation ResolveOwnerType(Type type)
    {
        if (_results.TryGetValue(type, out var result))
        {
            return result;
        }

        result = new TypeDocumentation(_source, type, node: null);
        
        _results.Add(type, result);
        
        return result;
    }
}