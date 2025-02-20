using System.Reflection;

namespace BitzArt.XDoc;

/// <summary>
/// Contains information about XML documentation.
/// </summary>
public interface IXmlInfo
{
    /// <summary>
    /// Fetches XML documentation for the specified <see cref="Assembly"/>.
    /// </summary>
    /// <param name="assembly">The <see cref="Assembly"/> to retrieve documentation for.</param>"/>
    /// <returns><see cref="AssemblyXmlInfo"/> for the specified <see cref="Assembly"/>.</returns>
    public AssemblyXmlInfo GetData(Assembly assembly);

    /// <summary>
    /// Fetches XML documentation for the specified <see cref="Type"/>.
    /// </summary>
    /// <param name="type">The <see cref="Type"/> to retrieve documentation for.</param>"/>
    /// <returns><see cref="TypeXmlInfo"/> for the specified <see cref="Type"/>.</returns>
    public TypeXmlInfo? GetData(Type type);

    /// <summary>
    /// Fetches XML documentation for the specified <see cref="PropertyInfo"/>.
    /// </summary>
    /// <param name="property">The <see cref="PropertyInfo"/> to retrieve documentation for.</param>
    /// <returns><see cref="PropertyXmlInfo"/> for the specified <see cref="PropertyInfo"/>.</returns>
    public PropertyXmlInfo GetData(PropertyInfo property);
}

/// <summary>
/// Contains information about XML documentation of a specific object.
/// </summary>
/// <typeparam name="TProprietor">Type of the object that owns the XML documentation.</typeparam>
public interface IXmlInfo<TProprietor> : IXmlInfo
{
    /// <summary>
    /// The object that owns the XML documentation.
    /// </summary>
    public TProprietor Proprietor { get; }
}
