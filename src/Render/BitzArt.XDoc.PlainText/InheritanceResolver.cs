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
            _ => FindTargetMember(sourceMember.DeclaringType!, sourceMember, false)
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
            if (CheckPresence(type, sourceMember, out var found))
            {
                return found;
            }
        }

        if (type.BaseType != null)
        {
            if (CheckPresence(type.BaseType, sourceMember, out var foundInBaseType))
            {
                return foundInBaseType;
            }
            
            var result = FindTargetMember(type.BaseType, sourceMember, true);

            if (result is not null)
            {
                return result;
            }
        }

        foreach (var immediateInterface in type.GetImmediateInterfaces())
        {
            if (CheckPresence(immediateInterface, sourceMember, out var found))
            {
                return found;
            }
        
            var result = FindTargetMember(immediateInterface, sourceMember, true);

            if (result is not null)
            {
                return result;
            }
        }

        return null;
    }

    private static bool CheckPresence(Type type, MemberInfo sourceMember, out MemberInfo? found)
    {
        var memberInfos = type.GetMembers();

        foreach (var member in memberInfos)
        {
            if (member.MemberType != sourceMember.MemberType)
            {
                continue;
            }

            if (member.Name != sourceMember.Name)
            {
                continue;
            }

            if (sourceMember is MethodInfo sourceMethod && member is MethodInfo targetMethod)
            {
                var sourceParams = sourceMethod.GetParameters();
                var targetParams = targetMethod.GetParameters();

                if (sourceParams.Length != targetParams.Length)
                {
                    continue;
                }

                var parametersMatch = true;

                for (var i = 0; i < sourceParams.Length; i++)
                {
                    if (sourceParams[i].ParameterType != targetParams[i].ParameterType)
                    {
                        parametersMatch = false;
                        break;
                    }
                }

                if (!parametersMatch)
                {
                    continue;
                }
            }

            found = member;

            return true;
        }

        found = null;

        return false;
    }
}