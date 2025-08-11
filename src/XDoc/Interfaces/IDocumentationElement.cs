namespace BitzArt.XDoc;

/// <summary>
/// Contains documentation for a specific value of type <typeparamref name="TTarget"/>.
/// </summary>
/// <typeparam name="TTarget">Type of the documentation owner.</typeparam>
public interface IDocumentationElement<TTarget>
    where TTarget : class
{
    /// <summary>
    /// Target of this documentation.
    /// </summary>
    public TTarget Target { get; }
}
