namespace Xdoc.Renderer.PlaintText;

public class PlainTextRenderer
{
    private readonly IDocumentStore _documentStore;

    public PlainTextRenderer(IDocumentStore documentStore)
    {
        _documentStore = documentStore;
    }

    public string Render(ISummarized? summarized)
    {
        var value = summarized?.Summary.Xml?.InnerText?.Trim();

        return value ?? string.Empty;
    }
}