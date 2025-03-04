using System.Collections.Immutable;
using System.Reflection;

namespace BitzArt.XDoc.Resolvers;

public class InheritanceResolver
{
    private IXDoc _source;

    private InheritanceResolver(IXDoc documentationSource)
    {
        _source = documentationSource;
    }

    internal static InheritanceMemberDocumentationReference? ResolveInheritance(MemberDocumentation documentation)
    {
        var inheritanceResolver = new InheritanceResolver(documentation.Source);

        return inheritanceResolver.Resolve(documentation);
    }

    private InheritanceMemberDocumentationReference? Resolve(MemberDocumentation documentation)
    {
        if (documentation.Node == null || string.IsNullOrWhiteSpace(documentation.Node.InnerXml))
        {
            return null;
        }

        if (documentation is TypeDocumentation typeDocumentation)
        {
            return Resolve(typeDocumentation);
        }
        else if (documentation is MethodDocumentation methodDocumentation)
        {
            return Resolve(methodDocumentation);
        }
        else if (documentation is PropertyDocumentation propertyDocumentation)
        {
            return Resolve(propertyDocumentation);
        }
        else if (documentation is FieldDocumentation fieldDocumentation)
        {
            return Resolve(fieldDocumentation);
        }

        return ResolveGeneric(documentation);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="documentation"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    private InheritanceMemberDocumentationReference? ResolveGeneric(MemberDocumentation documentation)
    {
        if (!IsInherited(documentation))
        {
            return null;
        }

        return new InheritanceMemberDocumentationReference
        {
            RequirementNode = documentation.Node,
            TargetType = null
        };
    }

    private InheritanceMemberDocumentationReference? Resolve(TypeDocumentation typeDocumentation)
    {
        if (typeDocumentation.Type.BaseType is null ||
            string.IsNullOrWhiteSpace(typeDocumentation.Node?.InnerXml) ||
            !typeDocumentation.Node.InnerXml.Contains("<inheritdoc />"))
        {
            return null;
        }

        var baseTypeDocumentation = _source.Get(typeDocumentation.Type.BaseType);

        return CreateInheritanceMemberDocumentationReference(baseTypeDocumentation, typeDocumentation.Type.BaseType);
    }

    private InheritanceMemberDocumentationReference? Resolve(MethodDocumentation methodDocumentation)
    {
        if (!IsInherited(methodDocumentation))
        {
            return null;
        }

        var baseType = methodDocumentation.DeclaringType.BaseType;

        var baseMemberInfo = baseType?.GetMethod(methodDocumentation.Member.Name);

        if (baseMemberInfo is null)
        {
            return null;
        }

        var inheritedDocumentation = _source.Get(baseType!)?.GetDocumentation(baseMemberInfo);

        return CreateInheritanceMemberDocumentationReference(inheritedDocumentation, baseType);
    }

    private InheritanceMemberDocumentationReference? Resolve(PropertyDocumentation propertyDocumentation)
    {
        if (!IsInherited(propertyDocumentation))
        {
            return null;
        }

        var baseType = propertyDocumentation.DeclaringType.BaseType;

        var baseMemberInfo = baseType?.GetProperty(propertyDocumentation.Member.Name);

        if (baseMemberInfo is null)
        {
            return null;
        }

        var inheritedDocumentation = _source.Get(baseType!)?.GetDocumentation(baseMemberInfo);

        return CreateInheritanceMemberDocumentationReference(inheritedDocumentation, baseType);
    }

    private InheritanceMemberDocumentationReference? Resolve(FieldDocumentation fieldDocumentation)
    {
        if (!IsInherited(fieldDocumentation))
        {
            return null;
        }

        var baseType = fieldDocumentation.DeclaringType.BaseType;
        var baseMemberInfo = baseType?.GetField(fieldDocumentation.Member.Name);

        if (baseMemberInfo is null)
        {
            return null;
        }

        var inheritedDocumentation = _source.Get(baseType!)?.GetDocumentation(baseMemberInfo);

        return CreateInheritanceMemberDocumentationReference(inheritedDocumentation, baseType);
    }

    private static bool IsInherited(MemberDocumentation memberDocumentation)
    {
        return memberDocumentation.Node?.InnerXml.Contains("<inheritdoc />") ?? false;
    }

    private static InheritanceMemberDocumentationReference? CreateInheritanceMemberDocumentationReference(
        MemberDocumentation? memberDocumentation,
        Type? targetType)
    {
        if (memberDocumentation is null)
        {
            return null;
        }

        return new InheritanceMemberDocumentationReference
        {
            RequirementNode = memberDocumentation.Node,
            TargetType = targetType
        };
    }
}