using System.Reflection;

namespace BitzArt.XDoc;

internal class InheritanceResolver
{
    private readonly XDoc _xdoc;

    public InheritanceResolver(XDoc xdoc)
    {
        _xdoc = xdoc;
    }

    public DocumentationElement? GetDocumentationElement(MemberInfo target)
    {
        DocumentationElement? documentationElement;

        if (target is Type type)
        {
            documentationElement = FindTypeDocumentationElement(type);
        }
        else
        {
            documentationElement = FindMemberDocumentationElement(target);
        }

        return documentationElement;
    }

    private DocumentationElement? FindTypeDocumentationElement(Type type)
    {
        var parents = type.GetParents();

        foreach (var parent in parents)
        {
            var documentation = _xdoc.Get(parent);

            if (documentation is { Node: not null })
            {
                return documentation;
            }
        }

        foreach (var parent in parents)
        {
            var inheritedComment = FindTypeDocumentationElement(parent);

            if (inheritedComment != null)
            {
                return inheritedComment;
            }
        }

        return null;
    }

    private DocumentationElement? FindMemberDocumentationElement(MemberInfo memberInfo)
    {
        var documentation = FindMemberDocumentationElement(memberInfo, memberInfo.ReflectedType!);

        return documentation;
    }

    private DocumentationElement? FindMemberDocumentationElement(MemberInfo memberInfo, Type owner)
    {
        var parents = owner.GetParents();

        foreach (var parent in parents)
        {
            var memberFromTheParent = parent.GetMemberWithSameSignature(memberInfo);

            if (memberFromTheParent is null)
            {
                continue;
            }

            var documentation = _xdoc.Get(memberFromTheParent);

            if (documentation != null)
            {
                return documentation;
            }
        }

        foreach (var parent in parents)
        {
            var comment = FindMemberDocumentationElement(memberInfo, parent);

            if (comment != null)
            {
                return comment;
            }
        }

        return null;
    }
}