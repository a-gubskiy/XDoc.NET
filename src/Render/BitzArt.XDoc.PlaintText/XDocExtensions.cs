using BitzArt.XDoc;

namespace Xdoc.Renderer.PlaintText;

/// <summary>
/// Extension methods for <see cref="XDoc"/> objects which allow rendering documentation in plain text.
/// </summary>
public static class XDocExtensions
{

    /// <summary>
    /// Get full representation of the type documentation in plain text.
    /// </summary>
    /// <param name="documentation"></param>
    /// <returns></returns>
    public static string ToPlainText(this TypeDocumentation documentation)
    {
        var parsedContent = documentation.ParsedContent;
        
        return ToPlainText(parsedContent);
    }
    
    /// <summary>
    /// Get full representation of the property documentation in plain text.
    /// </summary>
    /// <param name="documentation"></param>
    /// <returns></returns>
    public static string ToPlainText(this PropertyDocumentation documentation)
    {
        var parsedContent = documentation.ParsedContent;
        
        return ToPlainText(parsedContent);
    }

    private static string ToPlainText(ParsedContent parsedContent)
    {
        throw new NotImplementedException();
    }
}