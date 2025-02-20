using System.Reflection;
using System.Xml;
using JetBrains.Annotations;

namespace Xdoc;

[PublicAPI]
public interface IDocumentStore
{
    /// <summary>
    /// Get class information for a given type.
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    XmlSummary? GetClassInfo(Type type);
    
    /// <summary>
    /// Get property information for a given type and property name.
    /// </summary>
    /// <param name="type"></param>
    /// <param name="propertyName"></param>
    /// <returns></returns>
    XmlSummary? GetPropertyInfo(Type type, string propertyName);
    
    /// <summary>
    /// List of assemblies loaded into the document store.
    /// </summary>
    IReadOnlyDictionary<string, AssemblyXmlInfo> Assemblies { get; }
}

public class DocumentStore : IDocumentStore
{
    private readonly Dictionary<string, AssemblyXmlInfo> _assemblies;

    public IReadOnlyDictionary<string, AssemblyXmlInfo> Assemblies => _assemblies;

    public DocumentStore()
    {
        _assemblies = new Dictionary<string, AssemblyXmlInfo>();
    }
    
    public XmlSummary? GetClassInfo(Type type)
    {
        var assemblyInfo = GetAssemblyXmlInfo(type);
        
        var xpath = $"/doc/members/member[@name='T:{type.FullName}']";
        var typeNode = assemblyInfo.Xml.SelectSingleNode(xpath);

        if (typeNode != null)
        {
            var inheritdoc = typeNode.SelectSingleNode("inheritdoc");

            if (inheritdoc != null && type.BaseType != null)
            {
                var comment = GetClassInfo(type.BaseType);

                return comment;
            }

            return new XmlSummary(typeNode);
        }

        return null;
    }

    public XmlSummary? GetPropertyInfo(Type type, string propertyName)
    {
        var assemblyInfo = GetAssemblyXmlInfo(type);
        
        var xpath = $"/doc/members/member[@name='P:{type.FullName}.{propertyName}']";
        var propertyNode = assemblyInfo.Xml.SelectSingleNode(xpath);

        if (propertyNode != null)
        {
            var inheritdoc = propertyNode.SelectSingleNode("inheritdoc");

            if (inheritdoc != null && type.BaseType != null)
            {
                var summary = GetPropertyInfo(type.BaseType, propertyName);
                
                return summary;
            }

            return new XmlSummary(propertyNode);
        }

        return null;
    }

    private AssemblyXmlInfo GetAssemblyXmlInfo(Type type)
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