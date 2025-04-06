using BitzArt.XDoc.PlainText;

namespace BitzArt.XDoc;

/// <summary>
/// Extension methods for <see cref="XDoc"/> objects which allow rendering documentation in plain text.
/// </summary>
public static class XDocPlainTextExtensions
{
    /// <summary>
    /// Renders the documentation of a <see cref="Type"/> as plain text.
    /// </summary>
    /// <param name="documentation">The documentation element to render.</param>
    /// <param name="configureOptions">Optional configuration for the renderer.</param>
    /// <returns></returns>
    public static string ToPlainText(this DocumentationElement documentation, Action<RendererOptions>? configureOptions = null)
    {
        var options = new RendererOptions();
        configureOptions?.Invoke(options);

        return new PlainTextRenderer(documentation.Source, options).Render(documentation);
    }
}