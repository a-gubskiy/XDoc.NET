using System.Reflection;

namespace BitzArt.XDoc;

/// <summary>
/// Provides functionality for fetching and parsing documentation
/// of code members.
/// <para>
/// Currently supported code members: <br />
/// – <see cref="Assembly"/> <br />
/// – <see cref="Type"/> <br />
/// – <see cref="PropertyInfo"/>
/// </para>
/// <para>
/// Fetching documentation for an <see cref="Assembly"/> or any of its members
/// will result in parsing and indexing of the XML documentation file provided for said <see cref="Assembly"/>.
/// </para>
/// <para>
/// As long as you retain an instance of an <see cref="XDoc"/> object,
/// all data fetched by it will be parsed, cached, indexed and reused
/// for subsequent requests. <br />
/// Disposing of an <see cref="XDoc"/> object will lead to a loss of all said data.
/// </para>
/// </summary>
public class XDoc : IXDoc
{
    private readonly Dictionary<Assembly, AssemblyDocumentation> _collectedAssemblies = [];

    /// <inheritdoc />
    public AssemblyDocumentation Get(Assembly assembly)
        => _collectedAssemblies.TryGetValue(assembly, out var result)
            ? result
            : Collect(assembly);

    private AssemblyDocumentation Collect(Assembly assembly)
    {
        var result = new AssemblyDocumentation(this, assembly);
        _collectedAssemblies.Add(assembly, result);

        return result;
    }

    /// <inheritdoc />
    public TypeDocumentation? Get(Type type)
        => Get(type.Assembly).GetDocumentation(type);

    /// <inheritdoc />
    public PropertyDocumentation? Get(PropertyInfo? property)
        => property is null ? null : Get(property.DeclaringType!)?.GetDocumentation(property);

    /// <inheritdoc />
    public MethodDocumentation? Get(MethodInfo? methodInfo)
        => methodInfo is null ? null : Get(methodInfo.DeclaringType!)?.GetDocumentation(methodInfo);

    /// <inheritdoc />
    public FieldDocumentation? Get(FieldInfo? fieldInfo)
        => fieldInfo is null ? null : Get(fieldInfo.DeclaringType!)?.GetDocumentation(fieldInfo);
}