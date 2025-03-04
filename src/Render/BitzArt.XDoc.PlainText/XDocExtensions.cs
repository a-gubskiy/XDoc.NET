using BitzArt.XDoc;

namespace Xdoc.Renderer.PlaintText;

/// <summary>
/// Extension methods for <see cref="XDoc"/> objects which allow rendering documentation in plain text.
/// </summary>
public static class XDocExtensions
{
    private static readonly IXmlRenderer Renderer = new SimpleXmlRenderer(); //new XmlRenderer();

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