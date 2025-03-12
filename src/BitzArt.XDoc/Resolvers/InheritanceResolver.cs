namespace BitzArt.XDoc.Resolvers;

/// <summary>
/// Resolves inheritance for member documentation.
/// </summary>
internal class InheritanceResolver
{
    private readonly XDoc _source;

    private InheritanceResolver(XDoc documentationSource)
    {
        _source = documentationSource;
    }

    internal static InheritanceMemberDocumentationReference? Resolve(MemberDocumentation documentation)
    {
        var inheritanceResolver = new InheritanceResolver(documentation.Source);

        return inheritanceResolver.ResolveInheritance(documentation);
    }

    private InheritanceMemberDocumentationReference? ResolveInheritance(MemberDocumentation documentation)
    {
        if (!IsInherited(documentation))
        {
            return null;
        }

        var result = documentation switch
        {
            TypeDocumentation typeDocumentation => ResolveInheritance(typeDocumentation),
            MethodDocumentation methodDocumentation => ResolveInheritance(methodDocumentation),
            PropertyDocumentation propertyDocumentation => ResolveInheritance(propertyDocumentation),
            FieldDocumentation fieldDocumentation => ResolveInheritance(fieldDocumentation),
            _ => new InheritanceMemberDocumentationReference(documentation.Node!)
        };

        return result;
    }

    private InheritanceMemberDocumentationReference? ResolveInheritance(TypeDocumentation typeDocumentation)
    {
        if (typeDocumentation.Type.BaseType is null)
        {
            return null;
        }

        var baseTypeDocumentation = _source.Get(typeDocumentation.Type.BaseType);

        return CreateInheritanceMemberDocumentationReference(baseTypeDocumentation);
    }

    private InheritanceMemberDocumentationReference? ResolveInheritance(MethodDocumentation methodDocumentation)
    {
        var baseType = methodDocumentation.DeclaringType.BaseType;
        var baseMemberInfo = baseType?.GetMethod(methodDocumentation.MemberName);

        if (baseMemberInfo is null)
        {
            return null;
        }

        var inheritedDocumentation = _source.Get(baseType!)?.GetDocumentation(baseMemberInfo);

        return CreateInheritanceMemberDocumentationReference(inheritedDocumentation);
    }

    private InheritanceMemberDocumentationReference? ResolveInheritance(PropertyDocumentation propertyDocumentation)
    {
        var baseType = propertyDocumentation.DeclaringType.BaseType;
        var baseMemberInfo = baseType?.GetProperty(propertyDocumentation.MemberName);

        if (baseMemberInfo is null)
        {
            return null;
        }

        var inheritedDocumentation = _source.Get(baseType)?.GetDocumentation(baseMemberInfo);

        return CreateInheritanceMemberDocumentationReference(inheritedDocumentation);
    }

    private InheritanceMemberDocumentationReference? ResolveInheritance(FieldDocumentation fieldDocumentation)
    {
        var baseType = fieldDocumentation.DeclaringType.BaseType;
        var baseMemberInfo = baseType?.GetField(fieldDocumentation.MemberName);

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