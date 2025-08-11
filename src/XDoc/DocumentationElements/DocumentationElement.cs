namespace XDoc;

/// <summary>
/// A base class for <see cref="XDoc"/> documentation elements.
/// </summary>
public abstract class DocumentationElement
{
    /// <summary>
    /// Source <see cref="XDoc"/> instance used to generate this <see cref="DocumentationElement"/>.
    /// </summary>
    internal XDoc Source { get; private init; }

    /// <summary>
    /// Initializes a new instance of the <see cref="DocumentationElement"/> class.
    /// </summary>
    /// <param name="source">The source of the documentation.</param>
    internal DocumentationElement(XDoc source)
    {
        Source = source;
    }
}