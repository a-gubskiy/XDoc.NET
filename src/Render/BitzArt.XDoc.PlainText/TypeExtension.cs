using System.Reflection;

namespace BitzArt.XDoc;

/// <summary>
/// Contains extension methods for <see cref="Type"/>.
/// </summary>
public static class TypeExtension
{
    /// <summary>
    /// Returns all parent types of the specified type.
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public static IReadOnlyCollection<Type> GetParents(this Type type)
    {
        var result = new List<Type>();

        if (type.BaseType != null)
        {
            result.Add(type.BaseType);
        }

        result.AddRange(type.GetInterfaces());

        return result.AsReadOnly();
    }

    /// <summary>
    /// Gets a member with the same signature as the specified member.
    /// </summary>
    /// <param name="type"></param>
    /// <param name="memberInfo"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public static MemberInfo? GetMemberWithSameSignature(this Type type, MemberInfo memberInfo)
    {
        var memberInfos = type.GetMember(memberInfo.Name).ToList();

        if (memberInfos.Count == 1)
        {
            return memberInfos[0];
        }

        if (memberInfos.Count == 0)
        {
            return null;
        }

        memberInfos = memberInfos
            .Where(m => IsSameSignature(m, memberInfo))
            .ToList();

        if (memberInfos.Count != 1)
        {
            throw new InvalidOperationException($"Cannot resolve member {memberInfo.Name} in {type.FullName}");
        }

        return memberInfos.First();
    }

    private static bool IsSameSignature(MemberInfo a, MemberInfo b)
    {
        if (a.MemberType != b.MemberType || a.Name != b.Name)
        {
            return false;
        }

        switch (a)
        {
            case MethodInfo methodA when b is MethodInfo methodB:
                var paramsA = methodA.GetParameters();
                var paramsB = methodB.GetParameters();
                return paramsA.Length == paramsB.Length &&
                       paramsA.Select(p => p.ParameterType)
                           .SequenceEqual(paramsB.Select(p => p.ParameterType));
            case PropertyInfo propertyA when b is PropertyInfo propertyB:
                return propertyA.PropertyType == propertyB.PropertyType;
            case FieldInfo fieldA when b is FieldInfo fieldB:
                return fieldA.FieldType == fieldB.FieldType;
            default:
                return false;
        }
    }
}