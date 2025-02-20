namespace Xdoc.Abstractions;

public interface IClassXmlInfo : ISummarized
{
    /// <summary>
    /// Documented properties of the class.
    /// </summary>
    IReadOnlyDictionary<string, IPropertyXmlInfo> Properties { get; }

    /// <summary>
    /// Class name.
    /// </summary>
    string Name { get; }

    /// <summary>
    /// Assembly which the class belongs to.
    /// </summary>
    IAssemblyXmlInfo Assembly { get; }

    /// <summary>
    /// Get information about a property.
    /// </summary>
    /// <param name="propertyName"></param>
    /// <returns></returns>
    IPropertyXmlInfo? GetPropertyInfo(string propertyName);
}