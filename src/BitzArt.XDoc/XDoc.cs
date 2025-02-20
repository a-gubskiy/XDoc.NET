using System.Reflection;

namespace BitzArt.XDoc;

/// <summary>
/// Provides functionality for fetching and parsing XML documentation
/// of assemblies, types, and properties.
/// <para>
/// As long as you retain an instance of an <see cref="XDoc"/> object,
/// all data fetched by it will be cached and reused
/// for subsequent requests.
/// </para>
/// <para>
/// Disposing of an <see cref="XDoc"/> object
/// will lead to a loss of all data cached by it.
/// </para>
/// </summary>
public class XDoc : IXmlInfo
{
    private readonly Dictionary<Assembly, AssemblyXmlInfo> _data = [];

    /// <inheritdoc/>
    public AssemblyXmlInfo GetData(Assembly assembly)
    {
        if (_data.TryGetValue(assembly, out var result))
        {
            return result;
        }

        result = new(this, assembly);
        _data.Add(assembly, result);

        return result;
    }

    /// <inheritdoc/>
    public TypeXmlInfo GetData(Type type)
    {
        var assemblyInfo = GetData(type.Assembly);
        var typeXmlInfo = assemblyInfo.GetData(type);

        return typeXmlInfo;
    }

    /// <inheritdoc/>
    public PropertyXmlInfo GetData(PropertyInfo property)
    {
        var declaringType = property.DeclaringType!;
        var typeXmlInfo = GetData(declaringType);
        var result = typeXmlInfo?.GetData(property);
        
        return result;
    }
}