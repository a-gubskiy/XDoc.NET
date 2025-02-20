using JetBrains.Annotations;
using Xdoc.Models;

namespace Xdoc;

[PublicAPI]
public interface IDocumentStore
{
    /// <summary>
    /// Get class information for a given type.
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    ClassXmlInfo? GetClassInfo(Type type);

    ClassXmlInfo? GetClassInfo<T>() => GetClassInfo(typeof(T));

    /// <summary>
    /// Get property information for a given type and property name.
    /// </summary>
    /// <param name="type"></param>
    /// <param name="propertyName"></param>
    /// <returns></returns>
    PropertyXmlInfo? GetPropertyInfo(Type type, string propertyName);

    PropertyXmlInfo? GetPropertyInfo<T>(string propertyName) => GetPropertyInfo(typeof(T), propertyName);

    /// <summary>
    /// List of assemblies loaded into the document store.
    /// </summary>
    IReadOnlyDictionary<string, AssemblyXmlInfo> Assemblies { get; }
}