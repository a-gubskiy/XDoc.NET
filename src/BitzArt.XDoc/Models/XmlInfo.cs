using System.Reflection;

namespace BitzArt.XDoc;

/// <summary>
/// Contains information about XML documentation.
/// </summary>
/// <typeparam name="TProprietor">Type of the object that owns the XML documentation.</typeparam>

public abstract class XmlInfo<TProprietor>(XDoc xdoc, TProprietor proprietor)
    : XmlInfo(xdoc), IXmlInfo<TProprietor>
{
    internal TProprietor Proprietor { get; private set; } = proprietor;

    TProprietor IXmlInfo<TProprietor>.Proprietor => Proprietor;
}

/// <inheritdoc cref="XmlInfo{TProprietor}"/>
public abstract class XmlInfo : IXmlInfo
{
    /// <summary>
    /// Initializes a new instance of <see cref="XmlInfo"/>.
    /// </summary>
    /// <param name="xdoc"><see cref="BitzArt.XDoc"/> instance.</param>
    internal XmlInfo(XDoc xdoc)
    {
        XDoc = xdoc;
    }
    internal XDoc XDoc { get; private set; }

    /// <inheritdoc/>
    public AssemblyXmlInfo GetData(Assembly assembly)
        => XDoc.GetData(assembly);

    /// <inheritdoc/>
    public TypeXmlInfo? GetData(Type type)
        => XDoc.GetData(type);

    /// <inheritdoc/>
    public PropertyXmlInfo GetData(PropertyInfo property)
        => XDoc.GetData(property);
}
