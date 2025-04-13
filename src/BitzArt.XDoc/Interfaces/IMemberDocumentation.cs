using System.Reflection;
using System.Xml;

namespace BitzArt.XDoc;

/// <summary>
/// Contains documentation for a specific <see cref="MemberInfo"/>
/// </summary>
public interface IMemberDocumentation
{
    /// <summary>
    /// The <see cref="MemberInfo"/> this documentation is provided for.
    /// </summary>
    public MemberInfo Member { get; }

    /// <summary>
    /// XML documentation node, if available.
    /// </summary>
    public XmlNode? Node { get; }

    /// <summary>
    /// Fetches the documentation element for the inheritance target of this member.
    /// </summary>
    /// <returns>Documentation element for the inheritance target.</returns>
    public IMemberDocumentation? GetInheritanceTargetDocumentation();

    /// <summary>
    /// Resolves the inheritance target for this member.
    /// </summary>
    /// <returns><see cref="MemberInfo"/> of the inheritance target, if available; otherwise, <see langword="null"/>.</returns>
    public MemberInfo? GetInheritanceTarget();
}
