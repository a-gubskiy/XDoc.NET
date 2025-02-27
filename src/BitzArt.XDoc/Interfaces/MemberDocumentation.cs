using BitzArt.XDoc.Resolvers;

namespace BitzArt.XDoc;

/// <summary>
/// All documentation classes should inherit from this class.
/// This class contains code applicable to any member documentation (both Type & MemberInfo).
/// </summary>
public abstract class MemberDocumentation
{
    // Documentation of a code member:
    // - Type as a member of an Assembly;
    // - MemberInfo as a member of Type.

    private bool _isResolved = false;
    private InheritanceMemberDocumentationReference? _inherited;
    private IReadOnlyCollection<MemberDocumentationReference>? _references;

    public InheritanceMemberDocumentationReference? Inherited
    {
        get
        {
            OnRequireResolve();

            return _inherited;
        }
    }

    public IReadOnlyCollection<MemberDocumentationReference> References
    {
        get
        {
            OnRequireResolve();

            return _references;
        }
    }

    private void OnRequireResolve()
    {
        if (_isResolved)
        {
            return;
        }

        _inherited = InheritanceResolver.Resolve(this);
        _references = CrefResolver.Resolve(this);

        _isResolved = true;
    }
}