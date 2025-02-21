using System.Reflection;
using System.Xml;

namespace BitzArt.XDoc;

internal static class XmlUtility
{
    /// <summary>
    /// Fetches XML documentation for the specified <see cref="Assembly"/>.<br/>
    /// </summary>
    /// <param name="source">The <see cref="XDoc"/> instance that is requesting the documentation.</param>
    /// <param name="assembly">The <see cref="Assembly"/> to fetch documentation for.</param>
    /// <returns>
    /// A Dictionary containing all assembly types as keys and their respective <see cref="TypeDocumentation"/> as values, if any.<br/>
    /// In case no documentation is found for a type, the respective value will be <see langword="null"/>.
    /// </returns>
    internal static Dictionary<Type, TypeDocumentation> Fetch(XDoc source, Assembly assembly)
    {
        try
        {
            var filePath = Path.ChangeExtension(assembly.Location, "xml");

            // 'using' statement ensures the file stream is disposed of after use
            using var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);

            var xmlDocument = new XmlDocument();
            xmlDocument.Load(fileStream);

            return Parse(source, assembly, xmlDocument);
        }
        catch (Exception ex)
        {
            throw new XDocException("Something went wrong while trying to parse the XML documentation file. " +
                "See inner exception for details.", ex);
        }
    }

    internal static Dictionary<Type, TypeDocumentation> Parse(XDoc source, Assembly assembly, XmlDocument xml)
    {
        var nodeList = xml.SelectNodes("/doc/members/member")
            ?? throw new InvalidOperationException("No documentation found in the XML file.");

        Dictionary<Type, TypeDocumentation> results = new(nodeList.Count);

        foreach (XmlNode node in nodeList)
        {
            Parse(source, node, assembly, results);
        }

        return results;
    }

    private static void Parse(XDoc source, XmlNode node, Assembly assembly, Dictionary<Type, TypeDocumentation> results)
    {
        if (node.Attributes is null || node.Attributes.Count == 0) throw new InvalidOperationException("Invalid XML node.");

        var name = node.Attributes["name"]?.Value
            ?? throw new InvalidOperationException($"No 'name' attribute found in XML node '{node.Value}'.");

        switch (name[0] )
        {
            case 'T': ParseTypeNode(source, node, assembly, name[2..], results); break;
            case 'P': ParsePropertyNode(source, node, assembly, name[2..], results); break;
            case 'M': break; //ParseMethodNode(source, node, assembly, name[2..], results); break;
            default: break;
        };
    }

    private static TypeDocumentation ParseTypeNode(XDoc source, XmlNode node, Assembly assembly, string name, Dictionary<Type, TypeDocumentation> results)
    {
        var type = assembly.GetType(name)
            ?? throw new InvalidOperationException($"Type '{name}' not found.");

        // We could handle this case by finding and updating the existing object,
        // but I don't see a reason why this could be necessary.
        if (results.ContainsKey(type)) throw new InvalidOperationException("Invalid XML. Type nodes should always go first in the XML file");

        var typeDocumentation = new TypeDocumentation(source, type, node);

        results[type] = typeDocumentation;

        return typeDocumentation;
    }

    private static PropertyDocumentation ParsePropertyNode(XDoc source, XmlNode node, Assembly assembly, string name, Dictionary<Type, TypeDocumentation> results)
    {
        var index = name.LastIndexOf('.');
        if (index == -1) throw new InvalidOperationException("Encountered invalid XML node.");

        var (typeName, memberName) = (name[..index], name[(index + 1)..]);

        var type = assembly.GetType(typeName)
            ?? throw new InvalidOperationException($"Type '{typeName}' not found.");

        var propertyInfo = type.GetProperty(memberName)
            ?? throw new InvalidOperationException($"Property '{memberName}' not found in type '{typeName}'.");

        var typeDocumentation = ResolveOwnerType(source, type, results);

        var propertyDocumentation = new PropertyDocumentation(source, typeDocumentation, propertyInfo, node);

        typeDocumentation.MemberData.Add(propertyInfo, propertyDocumentation);

        return propertyDocumentation;
    }

    private static TypeDocumentation ResolveOwnerType(XDoc source, Type type, Dictionary<Type, TypeDocumentation> results)
    {
        if (results.TryGetValue(type, out var result))
        {
            return result;
        }

        result = new TypeDocumentation(source, type, node: null);
        results.Add(type, result);
        return result;
    }
}
