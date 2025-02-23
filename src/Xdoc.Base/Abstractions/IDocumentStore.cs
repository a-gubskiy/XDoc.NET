using JetBrains.Annotations;
using Xdoc.Models;

namespace Xdoc.Abstractions;

/// <summary>
/// Interface for a document store.
/// </summary>
[PublicAPI]
public interface IDocumentStore
{
    /// <summary>
    /// Get class information for a given type.
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    ClassXmlInfo? GetClassInfo(Type type);

    /// <summary>
    /// Get class information for a given type.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    IClassXmlInfo? GetClassInfo<T>() => GetClassInfo(typeof(T));

    /// <summary>
    /// Get property information for a given type and property name.
    /// </summary>
    /// <param name="type"></param>
    /// <param name="propertyName"></param>
    /// <returns></returns>
    IPropertyXmlInfo? GetPropertyInfo(Type type, string propertyName);

    /// <summary>
    /// Get property information for a given type and property name.
    /// </summary>
    /// <param name="propertyName"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    IPropertyXmlInfo? GetPropertyInfo<T>(string propertyName) => GetPropertyInfo(typeof(T), propertyName);

    /// <summary>
    /// List of assemblies loaded into the document store.
    /// </summary>
    IReadOnlyDictionary<string, AssemblyXmlInfo> Assemblies { get; }
}