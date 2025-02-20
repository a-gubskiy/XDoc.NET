using System.Reflection;
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
    ClassXmlInfo GetClassInfo(Type type);
    
    /// <summary>
    /// Get property information for a given type and property name.
    /// </summary>
    /// <param name="type"></param>
    /// <param name="propertyName"></param>
    /// <returns></returns>
    PropertyXmlInfo GetPropertyInfo(Type type, string propertyName);
    
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
    
    public ClassXmlInfo GetClassInfo(Type type)
    {
        var assemblyXmlInfo = GetAssemblyXmlInfo(type);
        var classXmlInfo = assemblyXmlInfo.Get(type);
        
        return classXmlInfo;
    }

    public PropertyXmlInfo GetPropertyInfo(Type type, string propertyName)
    {
        var assemblyXmlInfo = GetAssemblyXmlInfo(type);
        var propertyXmlInfo = assemblyXmlInfo.Get(type, propertyName);

        return propertyXmlInfo;
    }

    private AssemblyXmlInfo GetAssemblyXmlInfo(Type type)
    {
        var assemblyName = type.Assembly.GetName();
        var key = assemblyName.Name ?? string.Empty;

        if (_assemblies.TryGetValue(key, out var a))
        {
            return a;
        }

        var assembly = LoadAssembly(type.Assembly);
            
        _assemblies[key] = assembly;

        return assembly;
    }

    public AssemblyXmlInfo LoadAssembly(Assembly assembly)
    {
        var pathToXmlDocumentation = Path.ChangeExtension(assembly.Location, "xml");

        if (File.Exists(pathToXmlDocumentation))
        {
            var xml = File.ReadAllText(pathToXmlDocumentation);
            var assemblyName = assembly.GetName();
            
            var assemblyXmlInfo = new AssemblyXmlInfo(assemblyName.Name ?? "", xml);
            
            return assemblyXmlInfo;
        }

        throw new FileNotFoundException($"XML documentation file not found: {pathToXmlDocumentation}");
    }
}