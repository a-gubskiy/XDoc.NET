namespace BitzArt.XDoc.Resolvers;

/// <summary>
/// Resolves inheritance for member documentation.
/// </summary>
internal class InheritanceResolver
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
    /// Resolves generic member documentation if it is inherited.
    /// </summary>
    /// <param name="documentation">The member documentation to resolve.</param>
    /// <returns>
    /// An <see cref="InheritanceMemberDocumentationReference"/> if the documentation is inherited; otherwise, null.
    /// </returns>
    private InheritanceMemberDocumentationReference? ResolveGeneric(MemberDocumentation documentation)
    {
        if (!IsInherited(documentation))
        {
            return null;
        }

        return new InheritanceMemberDocumentationReference(documentation.Node!);
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

        return CreateInheritanceMemberDocumentationReference(baseTypeDocumentation);
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

        return CreateInheritanceMemberDocumentationReference(inheritedDocumentation);
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

        return CreateInheritanceMemberDocumentationReference(inheritedDocumentation);
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

        return CreateInheritanceMemberDocumentationReference(inheritedDocumentation);
    }

    /// <summary>
    /// Determines if the member documentation is inherited.
    /// </summary>
    /// <param name="memberDocumentation">The member documentation to check.</param>
    /// <returns>
    /// True if the member documentation is inherited; otherwise, false.
    /// </returns>
    private static bool IsInherited(MemberDocumentation memberDocumentation)
    {
        return memberDocumentation.Node?.InnerXml.Contains("<inheritdoc />") ?? false;
    }

    private static InheritanceMemberDocumentationReference? CreateInheritanceMemberDocumentationReference(
        MemberDocumentation? memberDocumentation)
    {
        if (memberDocumentation is null)
        {
            return null;
        }

        return new InheritanceMemberDocumentationReference(memberDocumentation.Node!);
    }
}