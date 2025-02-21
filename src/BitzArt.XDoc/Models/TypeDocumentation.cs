using System.Reflection;
using System.Xml;
using System.Xml.Linq;

namespace BitzArt.XDoc;

/// <summary>
/// Holds information about documentation of a <see cref="System.Type"/>.
/// </summary>
public sealed class TypeDocumentation
{
    internal Dictionary<MemberInfo, object> MemberData;

    internal XDoc Source { get; private set; }

    internal XmlNode? Node { get; set; }

    /// <summary>
    /// The <see cref="Type"/> this documentation if provided for.
    /// </summary>
    public Type Type { get; private set; }

    internal TypeDocumentation(XDoc source, Type type, XmlNode? node)
    {
        Source = source;
        Type = type;
        Node = node;

        MemberData = [];
    }

    /// <summary>
    /// Gets the documentation for a <see cref="PropertyInfo"/> declared by this <see cref="Type"/>.
    /// </summary>
    /// <param name="property">The <see cref="PropertyInfo"/> to retrieve documentation for.</param>
    /// <returns><see cref="PropertyDocumentation"/> for the specified <see cref="PropertyInfo"/> if available; otherwise, <see langword="null"/>.</returns>
    public PropertyDocumentation? GetDocumentation(PropertyInfo property)
        => (PropertyDocumentation?)GetDocumentation<PropertyInfo>(property);

    /// <summary>
    /// Gets the documentation for a <see cref="MethodInfo"/> declared by this <see cref="Type"/>.
    /// </summary>
    /// <param name="method">The <see cref="MethodInfo"/> to retrieve documentation for.</param>
    /// <returns><see cref="MethodDocumentation"/> for the specified <see cref="MethodInfo"/> if available; otherwise, <see langword="null"/>.</returns>
    public MethodDocumentation? GetDocumentation(MethodInfo method)
        => (MethodDocumentation?)GetDocumentation<MethodInfo>(method);

    /// <summary>
    /// Gets the documentation for a <see cref="FieldInfo"/> declared by this <see cref="Type"/>.
    /// </summary>
    /// <param name="field">The <see cref="FieldInfo"/> to retrieve documentation for.</param>
    /// <returns><see cref="FieldDocumentation"/> for the specified <see cref="FieldInfo"/> if available; otherwise, <see langword="null"/>.</returns>
    public FieldDocumentation? GetDocumentation(FieldInfo field)
        => (FieldDocumentation?)GetDocumentation<FieldInfo>(field);

    private object? GetDocumentation<TMember>(TMember property)
        where TMember : MemberInfo
         => MemberData.TryGetValue(Validate(property), out var result)
            ? result
            : null;

    private TMember Validate<TMember>(TMember property)
        where TMember : MemberInfo
    {
        if (property.DeclaringType != Type) throw new InvalidOperationException("The provided property is not defined in this type.");
        return property;
    }

    /// <inheritdoc/>
    public override string ToString() => $"{nameof(TypeDocumentation)} for {Type.Name!}";
}