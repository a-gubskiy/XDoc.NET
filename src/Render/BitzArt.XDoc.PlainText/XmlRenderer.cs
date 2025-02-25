using System;
using System.Linq;
using System.Text;
using System.Xml;

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

        var text = ProcessNode(xmlNode);
        return Normalize(text);
    }

    /// <summary>
    /// Normalize the input string by removing extra empty lines and trimming each line.
    /// </summary>
    public static string Normalize(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            return string.Empty;
        }

        var lines = input.Split('\n')
            .Select(line => line.TrimEnd())
            .Where(line => !string.IsNullOrWhiteSpace(line))
            .ToArray();

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
            string childText = ProcessChildren(element).Trim();

            switch (element.Name)
            {
                case "see":
                case "seealso":
                    if (crefAttribute != null)
                    {
                        var crefValue = crefAttribute.Value;
                        if (crefValue.StartsWith("T:"))
                        {
                            crefValue = crefValue.Substring(2);
                        }
                        builder.Append(crefValue);
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
                    builder.Append("\n(Parameter: " + nameAttribute?.Value + ") ");
                    builder.Append(childText);
                    builder.AppendLine();
                    break;

                case "returns":
                    builder.Append("\nReturns: " + childText);
                    builder.AppendLine();
                    break;

                case "exception":
                    builder.Append("\nThrows ");
                    if (crefAttribute != null)
                    {
                        var value = crefAttribute.Value;
                        if (value.StartsWith("T:"))
                        {
                            value = value.Substring(2);
                        }
                        builder.Append(value);
                    }
                    builder.Append(": " + childText);
                    builder.AppendLine();
                    break;

                case "value":
                    builder.Append("\nValue: " + childText);
                    builder.AppendLine();
                    break;

                case "para":
                    builder.AppendLine();
                    builder.AppendLine(childText);
                    builder.AppendLine();
                    break;

                case "remarks":
                    builder.AppendLine();
                    builder.AppendLine(childText);
                    break;

                case "list":
                    builder.AppendLine();
                    var listType = element.Attributes["type"]?.Value;
                    foreach (XmlNode child in element.ChildNodes)
                    {
                        if (child.Name == "item")
                        {
                            var itemText = ProcessChildren(child).Trim();
                            
                            string prefix = listType switch
                            {
                                "number" => "1. ",
                                "bullet" => "– ",
                                "table" => "* ",
                                _ => "– "
                            };
                            builder.AppendLine(prefix + itemText);
                        }
                    }
                    break;

                case "code":
                    builder.AppendLine();
                    builder.AppendLine("```");
                    builder.AppendLine(childText);
                    builder.AppendLine("```");
                    builder.AppendLine();
                    break;

                case "example":
                    builder.AppendLine();
                    builder.AppendLine(childText);
                    builder.AppendLine();
                    break;

                case "c":
                    builder.Append("`" + childText + "`");
                    break;

                default:
                    builder.Append(childText);
                    break;
            }
        }

        return builder.ToString();
    }

    private string ProcessChildren(XmlNode node)
    {
        var builder = new StringBuilder();
        foreach (XmlNode child in node.ChildNodes)
        {
            builder.Append(ProcessNode(child));
        }
        return builder.ToString();
    }
}