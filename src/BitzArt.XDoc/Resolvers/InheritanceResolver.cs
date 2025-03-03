using System.Collections.Immutable;
using System.Reflection;

namespace BitzArt.XDoc.Resolvers;

public class InheritanceResolver
{
    private XDoc _source;

    private InheritanceResolver(XDoc documentationSource)
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

        throw new NotImplementedException();
    }

    private InheritanceMemberDocumentationReference? Resolve(TypeDocumentation typeDocumentation)
    {
        if (typeDocumentation.Type.BaseType is null)
        {
            return null;
        }

        var baseTypeDocumentation = _source.Get(typeDocumentation.Type.BaseType);

        return new InheritanceMemberDocumentationReference
        {
            RequirementNode = baseTypeDocumentation.Node,
            TargetType = typeDocumentation.Type.BaseType
        };
    }

    private InheritanceMemberDocumentationReference? Resolve(MethodDocumentation methodDocumentation)
    {
        if (!(methodDocumentation.Node?.InnerXml.Contains("<inheritdoc />") ?? false))
        {
            return null;
        }

        var declaringType = methodDocumentation.DeclaringType;
        var baseType = declaringType.BaseType;

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
        if (!(propertyDocumentation.Node?.InnerXml.Contains("<inheritdoc />") ?? false))
        {
            return null;
        }

        var declaringType = propertyDocumentation.DeclaringType;
        var baseType = declaringType.BaseType;

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
        if (!(fieldDocumentation.Node?.InnerXml.Contains("<inheritdoc />") ?? false))
        {
            return null;
        }

        var declaringType = fieldDocumentation.DeclaringType;
        var baseType = declaringType.BaseType;

        var baseMemberInfo = baseType?.GetField(fieldDocumentation.Member.Name);

        if (baseMemberInfo is null)
        {
            return null;
        }

        var inheritedDocumentation = _source.Get(baseType!)?.GetDocumentation(baseMemberInfo);

        return CreateInheritanceMemberDocumentationReference(inheritedDocumentation, baseType);
    }

    private static InheritanceMemberDocumentationReference? CreateInheritanceMemberDocumentationReference(
        MemberDocumentation? memberDocumentation,
        Type? baseType)
    {
        if (memberDocumentation is null)
        {
            return null;
        }

        return new InheritanceMemberDocumentationReference
        {
            RequirementNode = memberDocumentation.Node,
            TargetType = baseType
        };
    }
}