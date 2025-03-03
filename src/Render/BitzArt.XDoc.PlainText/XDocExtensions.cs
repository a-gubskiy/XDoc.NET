using System.Text;
using BitzArt.XDoc;
using Xdoc.Renderer.PlainText;

namespace Xdoc.Renderer.PlaintText;

/// <summary>
/// Extension methods for <see cref="XDoc"/> objects which allow rendering documentation in plain text.
/// </summary>
public static class XDocExtensions
{
    private static readonly XmlRenderer Renderer = new XmlRenderer();
    
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

        var builder = new StringBuilder();

        if (documentation.Inherited != null)
        {
            builder.AppendLine(ToPlainText(documentation.Inherited));
        }
        else
        {
            builder.AppendLine(Renderer.Render(documentation));
        }

        

        return builder.ToString().Trim();
    }

    private static string ToPlainText(MemberDocumentationReference reference)
    {
        throw new NotImplementedException();
    }

    private static string ResolveName(MemberDocumentation documentation)
    {
        throw new NotImplementedException();
    }
}