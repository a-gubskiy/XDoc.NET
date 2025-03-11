namespace BitzArt.XDoc.Resolvers;

/// <summary>
/// Resolves inheritance for member documentation.
/// </summary>
public class InheritanceResolver
{
    private readonly IXDoc _source;

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
        if (string.IsNullOrWhiteSpace(documentation?.Node?.InnerXml))
        {
            return null;
        }

        var result = documentation switch
        {
            TypeDocumentation typeDocumentation => Resolve(typeDocumentation),
            MethodDocumentation methodDocumentation => Resolve(methodDocumentation),
            PropertyDocumentation propertyDocumentation => Resolve(propertyDocumentation),
            FieldDocumentation fieldDocumentation => Resolve(fieldDocumentation),
            _ => ResolveGeneric(documentation)
        };

        return result;
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

        Type targetType;

        if (documentation is TypeDocumentation typeDocumentation)
        {
            targetType = typeDocumentation.Type;
        }
        else if (documentation is MethodDocumentation methodDocumentation)
        {
            targetType = methodDocumentation.DeclaringType;
        }
        else if (documentation is PropertyDocumentation propertyDocumentation)
        {
            targetType = propertyDocumentation.DeclaringType;
        }
        else if (documentation is FieldDocumentation fieldDocumentation)
        {
            targetType = fieldDocumentation.DeclaringType;
        }
        else
        {
            return null;
        }
        
        return new InheritanceMemberDocumentationReference(documentation.Node!, targetType);
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

        return CreateInheritanceMemberDocumentationReference(inheritedDocumentation, baseType!);
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

        var inheritedDocumentation = _source.Get(baseType)?.GetDocumentation(baseMemberInfo);

        return CreateInheritanceMemberDocumentationReference(inheritedDocumentation, baseType!);
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

        var inheritedDocumentation = _source.Get(baseType)?.GetDocumentation(baseMemberInfo);

        return CreateInheritanceMemberDocumentationReference(inheritedDocumentation, baseType!);
    }

    private static bool IsInherited(MemberDocumentation memberDocumentation)
    {
        return memberDocumentation.Node?.InnerXml.Contains("<inheritdoc />") ?? false;
    }

    private static InheritanceMemberDocumentationReference? CreateInheritanceMemberDocumentationReference(
        MemberDocumentation? memberDocumentation,
        Type targetType)
    {
        if (memberDocumentation is null)
        {
            return null;
        }

        return new InheritanceMemberDocumentationReference(memberDocumentation.Node!, targetType);
    }
}