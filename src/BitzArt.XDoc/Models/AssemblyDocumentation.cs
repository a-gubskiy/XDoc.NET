using System.Reflection;

namespace BitzArt.XDoc;

/// <summary>
/// Holds information about the documentation of an <see cref="System.Reflection.Assembly"/>.
/// </summary>
public sealed class AssemblyDocumentation
{
    /// <summary>
    /// The <see cref="System.Reflection.Assembly"/> this documentation is fetched for.
    /// </summary>
    private readonly Assembly _assembly;

    /// <summary>
    /// Type documentation found for this assembly.
    /// </summary>
    private readonly Dictionary<Type, TypeDocumentation> _typeData;

    internal AssemblyDocumentation(XDoc source, Assembly assembly)
    {
        _assembly = assembly;
        _typeData = XmlUtility.Fetch(source, assembly);
    }

    /// <summary>
    /// Gets the documentation for a <see cref="Type"/> defined in this assembly.
    /// </summary>
    /// <param name="type">The <see cref="Type"/> to retrieve documentation for.</param>
    /// <returns><see cref="TypeDocumentation"/> for the specified <see cref="Type"/> if available; otherwise, <see langword="null"/>.</returns>
    public TypeDocumentation? GetDocumentation(Type type)
        => _typeData.TryGetValue(Validate(type), out var result)
            ? result
            : null;

    private Type Validate(Type type)
    {
        if (type.Assembly != _assembly)
        {
            throw new InvalidOperationException("The provided type is not defined in this assembly.");
        }
        
        return type;
    }

    /// <inheritdoc/>
    public override string ToString() => $"{nameof(AssemblyDocumentation)} (Assembly:'{_assembly.GetName().Name!}')";
}