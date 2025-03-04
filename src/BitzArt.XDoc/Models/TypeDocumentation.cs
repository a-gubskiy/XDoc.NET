using System.Collections.Frozen;
using System.Reflection;
using System.Xml;

namespace BitzArt.XDoc;

/// <summary>
/// Holds information about documentation of a <see cref="System.Type"/>.
/// </summary>
public sealed class TypeDocumentation : MemberDocumentation
{
    private readonly Dictionary<MemberInfo, MemberDocumentation> _memberData;

    /// <summary>
    /// The <see cref="Type"/> this documentation if provided for.
    /// </summary>
    public Type Type { get; private init; }

    /// <summary>
    /// List of members declared by this <see cref="Type"/>.
    /// </summary>
    internal IReadOnlyDictionary<MemberInfo, MemberDocumentation> MemberData => _memberData.ToFrozenDictionary();

    internal TypeDocumentation(IXDoc source, Type type, XmlNode? node)
        : base(source, node)
    {
        Type = type;

        _memberData = [];
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

    private object? GetDocumentation<TMember>(TMember member)
        where TMember : MemberInfo
    {
        var memberInfo = Validate(member);

        return _memberData.GetValueOrDefault(memberInfo);
    }

    private TMember Validate<TMember>(TMember member)
        where TMember : MemberInfo
    {
        if (member.DeclaringType != Type)
        {
            throw new InvalidOperationException("The provided property is not defined in this type.");
        }

        if (member.DeclaringType != member.ReflectedType)
        {
            return member switch
            {
                PropertyInfo propertyInfo => (TMember)(MemberInfo)member.DeclaringType.GetProperty(propertyInfo.Name)!,
                // Method ...
            };
        }

        return member;
    }

    /// <inheritdoc/>
    public override string ToString() => $"{nameof(TypeDocumentation)} for {Type.Name!}";

    /// <summary>
    /// Adds documentation for a member declared by this <see cref="Type"/>.
    /// </summary>
    /// <param name="memberInfo"></param>
    /// <param name="documentation"></param>
    /// <typeparam name="T"></typeparam>
    internal void AddMemberData<T>(MemberInfo memberInfo, TypeMemberDocumentation<T> documentation)
        where T : MemberInfo
    {
        _memberData.Add(memberInfo, documentation);
    }
}