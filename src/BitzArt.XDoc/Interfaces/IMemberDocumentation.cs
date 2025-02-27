using System.Xml;

namespace BitzArt.XDoc;

public interface IDocumentation
{
    internal XmlNode? Node { get; }

    internal XDoc Source { get; }
}

internal interface ITypeDocumentation : IDocumentation
{
    Type Type { get; }
}

/// <summary>
/// Represents documentation for a member of a <see cref="Type"/>.
/// </summary>
internal interface IMemberDocumentation : IDocumentation
{
    /// <summary>
    /// The <see cref="Type"/> that declares the member.
    /// </summary>
    public Type DeclaringType { get; }
}