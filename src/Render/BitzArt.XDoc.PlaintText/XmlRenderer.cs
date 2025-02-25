using System.Xml;
using System.Text;

namespace Xdoc.Renderer.PlainText;

/// <summary>
/// Renders XML documentation to plain text.
/// <seealso href="https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/xmldoc/xml-documentation-comments">Microsoft specification</seealso>
/// </summary>
public class XmlRenderer
{
    public string Render(XmlNode? xmlNode)
    {
        if (xmlNode == null)
        {
            return string.Empty;
        }

        var text = Normalize(ProcessNode(xmlNode));
        return text;
    }

    /// <summary>
    /// Normalize the input string by removing empty lines and trimming each line.
    /// </summary>
    public static string Normalize(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            return string.Empty;
        }

        var lines = input.Split('\n')
            .Select(line => line.Trim())
            .Where(line => !string.IsNullOrWhiteSpace(line))
            .ToList();

        return string.Join("\n", lines);
    }

    private string ProcessNode(XmlNode node)
    {
        var builder = new StringBuilder();

        if (node is XmlText textNode)
        {
            builder.Append(textNode.Value);
        }
        else if (node is XmlElement element)
        {
            var crefAttribute = element.Attributes["cref"];
            var nameAttribute = element.Attributes["name"];

            switch (element.Name)
            {
                case "see":
                case "seealso":
                    if (crefAttribute != null)
                    {
                        var crefValue = crefAttribute.Value;
                        var className = crefValue.Split('.').Last();
                        builder.Append(className);
                    }
                    break;

                case "paramref":
                case "typeparamref":
                    if (nameAttribute != null)
                    {
                        builder.Append(nameAttribute.Value);
                    }
                    break;

                case "typeparam":
                    builder.Append($"(Type: {nameAttribute?.Value}) ");
                    break;

                case "param":
                    builder.Append($"(Parameter: {nameAttribute?.Value}) ");
                    ProcessChildren(element, builder);
                    break;

                case "returns":
                    builder.Append("Returns: ");
                    ProcessChildren(element, builder);
                    break;

                case "exception":
                    builder.Append("Throws ");
                    if (crefAttribute != null)
                    {
                        var value = crefAttribute.Value.Split('.').Last();
                        builder.Append(value);
                    }
                    builder.Append(": ");
                    ProcessChildren(element, builder);
                    break;

                case "value":
                    builder.Append("Value: ");
                    ProcessChildren(element, builder);
                    break;

                case "inheritdoc":
                    builder.Append("[Inherited documentation]");
                    break;

                case "include":
                    builder.Append("[Included documentation]");
                    break;

                case "list":
                    foreach (XmlNode child in element.ChildNodes)
                    {
                        if (child.Name == "item")
                        {
                            builder.Append("\n- ");
                            ProcessChildren(child, builder);
                        }
                    }
                    break;

                case "code":
                    builder.Append("\n");
                    ProcessChildren(element, builder);
                    builder.Append("\n");
                    break;

                case "c":
                    builder.Append("`");
                    ProcessChildren(element, builder);
                    builder.Append("`");
                    break;

                default:
                    ProcessChildren(element, builder);
                    break;
            }
        }

        return builder.ToString().Trim();
    }

    private void ProcessChildren(XmlNode node, StringBuilder builder)
    {
        foreach (XmlNode child in node.ChildNodes)
        {
            var childValue = ProcessNode(child);
            builder.Append(childValue);
            builder.Append(" ");
        }
    }
}