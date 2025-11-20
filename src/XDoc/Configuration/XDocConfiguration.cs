using System.Reflection;

namespace XDoc;

/// <summary>
/// Custom XDoc configuration.
/// </summary>
public class XDocConfiguration
{
    /// <summary>
    /// Implement custom documentation file path resolution.
    /// </summary>
    public Func<Assembly, string>? GetXmlDocumentationFilePath;
}