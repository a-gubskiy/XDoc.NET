namespace BitzArt.XDoc;

// This class is intended to replace IDocumentation interface
public abstract class MemberDocumentation
{
    // Documentation of a code member:
    // - Type as a member of an Assembly;
    // - MemberInfo as a member of Type.

    // All documentation classes should inherit from this class

    // Class contains code applicable to any member documentation (both Type & MemberInfo)

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

        // resolve Inherited & References
        // ...

        _isResolved = true;
    }
}