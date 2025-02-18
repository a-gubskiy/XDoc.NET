using System.Xml;

namespace Xdoc;

public class DocumentationStore
{
    private readonly IReadOnlyCollection<XmlDocument> _documents;

    public DocumentationStore(IReadOnlyCollection<XmlDocument> documents)
    {
        _documents = documents;
    }
}