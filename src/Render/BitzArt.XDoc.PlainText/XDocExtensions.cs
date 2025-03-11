namespace BitzArt.XDoc.PlainText;

/// <summary>
/// Extension methods for <see cref="XDoc"/> objects which allow rendering documentation in plain text.
/// </summary>
public static class XDocExtensions
{
    private static readonly PlainTextRenderer Renderer = new();

    /// <summary>
    /// Renders the documentation of a <see cref="Type"/> as plain text.
    /// </summary>
    /// <param name="documentation"></param>
    /// <returns></returns>
    public static string ToPlainText(this MemberDocumentation? documentation)
    {
        if (documentation == null)
        {
            return string.Empty;
        }

        return Renderer.Render(documentation);
    }
}