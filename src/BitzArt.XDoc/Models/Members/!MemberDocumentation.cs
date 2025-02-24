using System.Xml;

namespace BitzArt.XDoc;

/// <summary>
/// Contains documentation for a member of a <see cref="Type"/>.
/// </summary>
/// <typeparam name="TMember">Type of the member.</typeparam>
public abstract class MemberDocumentation<TMember>
    where TMember : class
{
    internal XDoc Source { get; private set; }
    internal TypeDocumentation ParentNode { get; private set; }

    private ParsedContent? _parsedContent;

    public ParsedContent ParsedContent => _parsedContent ??= Resolve();

    internal XmlNode Node { get; set; }

    public Type DeclaringType => ParentNode.Type;

    internal ParsedContent Resolve()
    {
        var builder = new ParsedContentBuilder();

        var parsedContent = builder.Build(this);

        return parsedContent;
    }

    /// <summary>
    /// The <typeparamref name="TMember"/> this documentation if provided for.
    /// </summary>
    public TMember Member { get; private set; }

    internal MemberDocumentation(XDoc source, TypeDocumentation parentNode, TMember member, XmlNode node)
    {
        Source = source;
        Member = member;
        ParentNode = parentNode;
        Node = node;
    }
}