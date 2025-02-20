namespace Xdoc.Renderer.PlaintText;

public class PlainTextRenderer
{
    public string Render(ISummarized summarized)
    {
        var value = summarized.Summary.Xml.InnerText!.Trim();

        return value;
    }
}