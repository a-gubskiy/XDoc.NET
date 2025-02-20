using Xdoc.Models;

namespace Xdoc;

/// <summary>
/// Interface for summarized objects.
/// </summary>
public interface ISummarized
{
    public XmlSummary Summary { get; }
}