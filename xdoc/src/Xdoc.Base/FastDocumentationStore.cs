using System.Reflection;
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
            var nodes = document.SelectNodes("/doc/members/member")?
                .Cast<XmlElement>()
                .ToList() ?? new List<XmlElement>();

            // foreach (var node in nodes)
            // {
            //     node.
            //     dictionary.Add();
            // }
            //
            //  nodes
            //     .Select(node => new
            //     {
            //         Key = node.GetAttribute("name"),
            //         Value = node.InnerText.Trim()
            //     })
            //     .ToList();
        }
        //
        // _xml = documents
        //     .SelectMany(document => 
        //     .ToDictionary(x => x.Key, x => x.Value);
    }

    public FastDocumentationStore(XmlDocument document)
        : this([document])
    {
    }

    public override string GetCommentForType(Type type)
    {
        var key = $"/doc/members/member[@name='T:{type.FullName}']";

        var value = _xml.GetValueOrDefault(key, string.Empty);

        return value;
    }

    public override string GetCommentForProperty(Type type, string propertyName)
    {
        var key = $"/doc/members/member[@name='P:{type.FullName}.{propertyName}']";

        var value = _xml.GetValueOrDefault(key, string.Empty);

        return value;
    }
}