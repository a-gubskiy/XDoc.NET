using System.Reflection;
using System.Xml;

namespace BitzArt.XDoc;

/// <summary>
/// Contains documentation for a member of a <see cref="Type"/>.
/// </summary>
/// <typeparam name="TMember">Type of the member.</typeparam>
public abstract class TypeMemberDocumentation<TMember> : MemberDocumentation
    where TMember : MemberInfo
{
    // private ParsedContent? _parsedContent;
    
    internal XDoc Source { get; private init; }

    internal TypeDocumentation ParentNode { get; private init; }

    // internal XmlNode Node { get; private init; }

    /// <summary>
    /// The <see cref="Type"/> that declares the member.
    /// </summary>
    public Type DeclaringType => ParentNode.Type;

    // public ParsedContent ParsedContent => _parsedContent ??= Resolve();


    // internal ParsedContent Resolve()
    // {
    //     var parsedContent = ParsedContentResolver.Resolve(this);
    //
    //     return parsedContent;
    // }

    /// <summary>
    /// The <typeparamref name="TMember"/> this documentation if provided for.
    /// </summary>
    public TMember Member { get; private set; }

    internal TypeMemberDocumentation(XDoc source, TypeDocumentation parentNode, TMember member, XmlNode node)
    {
        Source = source;
        Member = member;
        ParentNode = parentNode;
        Node = node;
    }
}