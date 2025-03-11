using System.Xml;
using BitzArt.XDoc.Resolvers;

namespace BitzArt.XDoc;

/// <summary>
/// All documentation classes should inherit from this class.
/// This class contains code applicable to any member documentation (both Type and MemberInfo).
/// </summary>
public abstract class MemberDocumentation
{
    // Documentation of a code member:
    // - Type as a member of an Assembly;
    // - MemberInfo as a member of Type.

    /// <summary>
    /// XML node that contains the documentation.
    /// </summary>
    public XmlNode? Node { get; private init; }

    internal IXDoc Source { get; private init; }

    private bool _isResolved = false;
    private InheritanceMemberDocumentationReference? _inherited;
    private IReadOnlyCollection<MemberDocumentationReference>? _references;

    /// <summary>
    /// Contains references to inherited documentation.
    /// </summary>
    public InheritanceMemberDocumentationReference? Inherited
    {
        get
        {
            OnRequireResolve();

            return _inherited;
        }
    }

    /// <summary>
    /// Contains references to other documentation.
    /// </summary>
    public IReadOnlyCollection<MemberDocumentationReference> References
    {
        get
        {
            OnRequireResolve();

            return _references!;
        }
    }
    
    
    /// <summary>
    /// Initializes a new instance of the <see cref="MemberDocumentation"/> class.
    /// </summary>
    /// <param name="source">The source of the documentation.</param>
    /// <param name="node">The XML node that contains the documentation.</param>
    protected MemberDocumentation(IXDoc source, XmlNode? node)
    {
        Source = source;
        Node = node;
    }

    /// <summary>
    /// Resolves the documentation.
    /// </summary>
    private void OnRequireResolve()
    {
        if (_isResolved)
        {
            return;
        }

        _inherited = InheritanceResolver.ResolveInheritance(this);
        _references = CrefResolver.Resolve(this);

        _isResolved = true;
    }
}