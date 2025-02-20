using System.Reflection;
using System.Xml;
using JetBrains.Annotations;
using Xdoc.Models;

namespace Xdoc;

public class DocumentStore : IDocumentStore
{
    private readonly Dictionary<string, AssemblyXmlInfo> _assemblies;

    public IReadOnlyDictionary<string, AssemblyXmlInfo> Assemblies => _assemblies;

    public DocumentStore()
    {
        _assemblies = new Dictionary<string, AssemblyXmlInfo>();
    }
    
    public ClassXmlInfo? GetClassInfo(Type type)
    {
        var assemblyInfo = GetAssemblyInfo(type);
        
        var xpath = $"/doc/members/member[@name='T:{type.FullName}']";
        var typeNode = assemblyInfo.Xml.SelectSingleNode(xpath);

        if (typeNode != null)
        {
            var inheritdoc = typeNode.SelectSingleNode("inheritdoc");

            if (inheritdoc != null && type.BaseType != null)
            {
                return GetClassInfo(type.BaseType);
            }

            return new ClassXmlInfo(type.FullName!, typeNode);
        }

        return null;
    }

    public PropertyXmlInfo? GetPropertyInfo(Type type, string propertyName)
    {
        var assemblyInfo = GetAssemblyInfo(type);
        
        var xpath = $"/doc/members/member[@name='P:{type.FullName}.{propertyName}']";
        var propertyNode = assemblyInfo.Xml.SelectSingleNode(xpath);

        if (propertyNode != null)
        {
            var inheritdoc = propertyNode.SelectSingleNode("inheritdoc");

            if (inheritdoc != null && type.BaseType != null)
            {
                return GetPropertyInfo(type.BaseType, propertyName);
            }

            return new PropertyXmlInfo(propertyName, propertyNode);
        }

        return null;
    }

    private AssemblyXmlInfo GetAssemblyInfo(Type type)
    {
        var assemblyName = type.Assembly.GetName();
        var key = assemblyName.Name ?? string.Empty;

        if (_assemblies.TryGetValue(key, out var a))
        {
            return a;
        }
        else
        {
            // Load new assembly
            
            var assembly = LoadAssemblyInfo(type.Assembly);

            _assemblies[key] = assembly;

            return assembly;
        }
    }

    /// <summary>
    /// Get XML documentation for an assembly.
    /// </summary>
    /// <param name="assembly"></param>
    /// <returns></returns>
    /// <exception cref="FileNotFoundException"></exception>
    public AssemblyXmlInfo LoadAssemblyInfo(Assembly assembly)
    {
        var pathToXmlDocumentation = Path.ChangeExtension(assembly.Location, "xml");

        if (File.Exists(pathToXmlDocumentation))
        {
            var xml = File.ReadAllText(pathToXmlDocumentation);
            var assemblyName = assembly.GetName();
            var assemblyXmlInfo = new AssemblyXmlInfo(assemblyName.Name!, xml);
            
            return assemblyXmlInfo;
        }

        throw new FileNotFoundException($"XML documentation file not found: {pathToXmlDocumentation}");
    }
}