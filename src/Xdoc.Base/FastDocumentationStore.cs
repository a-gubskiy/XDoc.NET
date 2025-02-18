using System.Collections.Frozen;
using System.Xml;

namespace Xdoc;

public class FastDocumentationStore : DocumentationStoreBase
{
    private readonly IReadOnlyDictionary<string, string> _xml;

    public FastDocumentationStore(IEnumerable<XmlDocument> documents)
    {
        var dictionary = new Dictionary<string, string>();

        foreach (var document in documents)
        {
            var nodes = document
                .SelectNodes("/doc/members/member")?
                .Cast<XmlNode>()
                .ToList() ?? [];

            foreach (var node in nodes)
            {
                var nameAttribute = node.Attributes?["name"];

                if (nameAttribute == null)
                {
                    continue;
                }

                dictionary.Add(nameAttribute.Value, node.InnerXml);
            }
        }

        _xml = dictionary.ToFrozenDictionary();
    }

    public FastDocumentationStore(XmlDocument document)
        : this([document])
    {
    }

    public override string GetCommentForType(Type type)
    {
        var key = $"T:{type.FullName}";

        var value = _xml.GetValueOrDefault(key, string.Empty);

        return value;
    }

    public override string GetCommentForProperty(Type type, string propertyName)
    {
        var key = $"P:{type.FullName}.{propertyName}";

        var value = _xml.GetValueOrDefault(key, string.Empty);

        return value;
    }
}