using System.Reflection;
using System.Xml;

namespace Xdoc;

public class XmlDocumentationStore : DocumentationStoreBase
{
    private readonly XmlDocument _documentation;

    public XmlDocumentationStore(XmlDocument documentation)
    {
        _documentation = documentation;
    }

    /// <inheritdoc />
    public override string GetCommentForType(Type type)
    {
        var xpath = $"/doc/members/member[@name='T:{type.FullName}']";
        var typeNode = _documentation.SelectSingleNode(xpath);

        if (typeNode != null)
        {
            var inheritdoc = typeNode.SelectSingleNode("inheritdoc");

            if (inheritdoc != null && type.BaseType != null)
            {
                var comment = GetCommentForType(type.BaseType);

                return comment;
            }
            else
            {
                var comment = typeNode.InnerText.Trim();

                return comment;
            }
        }

        return string.Empty;
    }

    /// <inheritdoc />
    public override string GetCommentForProperty(Type type, string propertyName)
    {
        var xpath = $"/doc/members/member[@name='P:{type.FullName}.{propertyName}']";
        var propertyNode = _documentation.SelectSingleNode(xpath);

        if (propertyNode != null)
        {
            var inheritdoc = propertyNode.SelectSingleNode("inheritdoc");

            if (inheritdoc != null && type.BaseType != null)
            {
                var comment = GetCommentForProperty(type.BaseType, propertyName);

                return comment;
            }
            else
            {
                var comment = propertyNode.InnerText.Trim();

                return comment;
            }
        }

        return string.Empty;
    }
}