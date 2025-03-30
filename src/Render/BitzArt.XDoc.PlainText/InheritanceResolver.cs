using System.Reflection;
using System.Xml;

namespace BitzArt.XDoc;

/// <summary>
/// Direct inheritance resolver (<c>&lt;inheritdoc/&gt;</c>)
/// </summary>
internal static class InheritanceResolver
{
    public static MemberInfo? GetTargetMember(MemberInfo sourceMember, XmlNode? node)
    {
        return sourceMember switch
        {
            Type type => FindTargetType(type),
            _ => FindTargetMember(sourceMember.DeclaringType!, sourceMember)
        };
    }

    private static MemberInfo? FindTargetType(Type type)
    {
        if (type.BaseType is not null)
        {
            return type.BaseType;
        }

        return type.GetImmediateInterfaces().FirstOrDefault();
    }

    private static MemberInfo? FindTargetMember(Type type, MemberInfo sourceMember, bool checkOwnMembers = false)
    {
        if (checkOwnMembers)
        {
            if (CheckPresence(type, out var found))
            {
                return found;
            }
        }

        foreach (var immediateInterface in type.GetImmediateInterfaces())
        {
            if (CheckPresence(immediateInterface, out var found))
            {
                return found;
            }
        }

        return FindTargetMember(type.BaseType!, sourceMember, true);
    }

    private static bool CheckPresence(Type type, out MemberInfo? found)
    {
        var memberInfos = type.GetMembers();

        foreach (var memberInfo in memberInfos)
        {
            switch (memberInfo)
            {
                case PropertyInfo propertyInfo:
                {
                    if (type.Has(propertyInfo))
                    {
                        found = memberInfo;
                        return true;
                    }

                    break;
                }
                case FieldInfo fieldInfo:
                {
                    if (type.Has(fieldInfo))
                    {
                        found = memberInfo;
                        return true;
                    }

                    break;
                }
                case MethodInfo methodInfo:
                {
                    if (type.Has(methodInfo))
                    {
                        found = memberInfo;
                        return true;
                    }

                    break;
                }
                default: throw new NotSupportedException("Unsupported member type");
            }
        }

        found = null;

        return false;
    }
}