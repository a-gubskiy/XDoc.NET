using System.Reflection;
using Xdoc.Models;

namespace Xdoc;

/// <summary>
/// Represents a document store for XML documentation.
/// </summary>
public class DocumentStore : IDocumentStore
{
    private readonly Dictionary<string, AssemblyXmlInfo> _assemblies;

    public IReadOnlyDictionary<string, AssemblyXmlInfo> Assemblies => _assemblies.AsReadOnly();

    public DocumentStore()
    {
        _assemblies = new Dictionary<string, AssemblyXmlInfo>();
    }

    public ClassXmlInfo? GetClassInfo(Type type)
    {
        var assemblyInfo = GetAssemblyInfo(type);
        var classXmlInfo = assemblyInfo.GetClassInfo(type);

        return classXmlInfo;
    }

    public PropertyXmlInfo? GetPropertyInfo(Type type, string propertyName)
    {
        var classXmlInfo = GetClassInfo(type);
        var propertyXmlInfo = classXmlInfo?.GetPropertyInfo(propertyName);
        
        return propertyXmlInfo;
    }

    public AssemblyXmlInfo GetAssemblyInfo(Type type)
    {
        var typeAssembly = type.Assembly;

        var assemblyName = typeAssembly.GetName();
        var key = assemblyName.Name ?? string.Empty;

        if (!_assemblies.ContainsKey(key))
        {
            // Load new assembly
            _assemblies[key] = LoadAssemblyInfo(typeAssembly);
        }

        return _assemblies[key];
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

        if (!File.Exists(pathToXmlDocumentation))
        {
            throw new XmlDocumentationFileNotFound(
                "XML documentation file not found. " +
                "Please check that xml documentation generation is enabled and documentation exist in assembly output directory.",
                pathToXmlDocumentation);
        }
        
        var xml = File.ReadAllText(pathToXmlDocumentation);
        var assemblyName = assembly.GetName();
        var assemblyXmlInfo = new AssemblyXmlInfo(assemblyName.Name!, xml, this);

        return assemblyXmlInfo;
    }
}