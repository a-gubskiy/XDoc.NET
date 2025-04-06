using System.Reflection;

namespace BitzArt.XDoc.PlainText;

/// <summary>
/// Direct inheritance resolver (<c>&lt;inheritdoc/&gt;</c>)
/// </summary>
internal static class InheritanceResolver
{
    public static MemberInfo? GetTargetMember(MemberInfo sourceMember)
    {
        return sourceMember switch
        {
            Type type => FindTargetType(type),
            MemberInfo => FindTargetMember(sourceMember.DeclaringType!, sourceMember)
        };
    }

    private static Type? FindTargetType(Type type)
    {
        if (type.BaseType is not null)
        {
            return type.BaseType;
        }

        return GetImmediateInterfaces(type).FirstOrDefault();
    }

    private static MemberInfo? FindTargetMember(Type type, MemberInfo sourceMember)
    {
        // 1. Check base type members
        if (type.BaseType is not null)
        {
            if (CheckPresence(type.BaseType, sourceMember, out var foundInBaseType))
            {
                return foundInBaseType;
            }
        }

        // 2. Check own immediate interfaces' members
        foreach (var immediateInterface in GetImmediateInterfaces(type))
        {
            if (CheckPresence(immediateInterface, sourceMember, out var found))
            {
                return found;
            }
        
            var result = FindTargetMember(immediateInterface, sourceMember);

            if (result is not null)
            {
                return result;
            }
        }

        // 3. Recursively visit base type (if any).
        if (type.BaseType is not null)
        {
            var result = FindTargetMember(type.BaseType, sourceMember);

            if (result is not null)
            {
                return result;
            }
        }
            

        return null;
    }

    private static bool CheckPresence(Type type, MemberInfo sourceMember, out MemberInfo? found)
    {
        found = sourceMember switch
        {
            PropertyInfo propertyInfo => type.GetProperty(propertyInfo.Name),

            FieldInfo fieldInfo => type.GetField(fieldInfo.Name),

            MethodInfo methodInfo => type.GetMethod(
                methodInfo.Name,
                methodInfo.GetGenericArguments().Length,
                [.. methodInfo.GetParameters().Select(p => p.ParameterType)]),

            _ => throw new NotSupportedException()
        };

        return found is not null;
    }

    private static IEnumerable<Type> GetImmediateInterfaces(Type type)
    {
        var inheritedInterfaces = type
            .GetInterfaces()
            .SelectMany(i => i.GetInterfaces())
            .Union(type.BaseType?.GetInterfaces() ?? []);

        var result = type.GetInterfaces().Except(inheritedInterfaces);

        return result;
    }
}