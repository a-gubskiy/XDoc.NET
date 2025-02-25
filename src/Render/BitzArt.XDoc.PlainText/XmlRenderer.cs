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

        var text = ProcessNode(xmlNode);

        // while (text.Contains("\n\n"))
        // {
        //     text = text.Replace("\n\n", "\n");
        // }

        return text;
        // var text = Normalize(ProcessNode(xmlNode));
        // return text;
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
            var textNodeValue = textNode.Value?.Trim() ?? "";
            
            builder.Append(textNodeValue);
        }
        else if (node is XmlElement element)
        {
            var crefAttribute = element.Attributes["cref"];
            var nameAttribute = element.Attributes["name"];
            var childText = ProcessChildren(element);

            childText = childText.Trim();
            
            switch (element.Name)
            {
                case "see":
                case "seealso":
                    if (crefAttribute != null)
                    {
                        try
                        {
                            var crefValue = crefAttribute.Value;

                            var className = crefValue.Split('.').Last();
                            builder.Append(className);
                        }
                        catch (XmlException ex)
                        {
                            return "[Invalid XML documentation]";
                        }
                    }

                    break;

                case "paramref":
                case "typeparamref":
                    if (nameAttribute != null)
                    {
                        builder.Append(" ").Append(nameAttribute.Value).Append(" ");
                    }

                    break;

                case "typeparam":
                    builder.Append($"(Type: {nameAttribute?.Value}) ");
                    break;

                case "param":
                    builder.Append($"\n(Parameter: {nameAttribute?.Value}) ");
                    builder.Append(childText);
                    builder.AppendLine();
                    break;

                case "returns":
                    builder.Append("\nReturns: ");
                    builder.Append(childText);
                    builder.AppendLine();
                    break;

                case "exception":
                    builder.Append("\nThrows ");
                    if (crefAttribute != null)
                    {
                        var value = crefAttribute.Value.Split('.').Last();
                        builder.Append(value);
                    }

                    builder.Append(": ");
                    builder.Append(childText);
                    builder.AppendLine();
                    break;

                case "value":
                    builder.Append("\nValue: ");
                    builder.Append(childText);
                    builder.AppendLine();
                    break;


                case "para":
                    builder.AppendLine();
                    builder.AppendLine();
                    builder.Append(childText);
                    builder.AppendLine();
                    builder.AppendLine();
                    break;

                case "remarks":
                    builder.AppendLine();
                    builder.Append(childText);
                    builder.AppendLine();
                    break;

                case "inheritdoc":
                    builder.Append("\n[Inherited documentation]\n");
                    break;

                case "include":
                    builder.Append("\n[Included documentation]\n");
                    break;

                case "list":
                    var listType = element.Attributes["type"]?.Value;
                    builder.AppendLine();

                    var itemCount = 1;

                    foreach (XmlNode child in element.ChildNodes)
                    {
                        if (child.Name == "item")
                        {
                            // Handle numbered/bullet/table lists differently
                            var prefix = listType switch
                            {
                                "number" => $"{itemCount++}. ",
                                "bullet" => "- ",
                                "table" => "* ",
                                _ => "- "
                            };

                            builder.Append(prefix);
                            builder.Append(childText);
                            builder.AppendLine();
                        }
                    }

                    break;

                case "code":
                    builder.Append("```");
                    builder.Append(childText);
                    builder.AppendLine("```");
                    break;

                case "example":
                    builder.AppendLine();
                    builder.Append(childText);
                    builder.AppendLine();
                    break;

                case "c":
                    builder.Append("`");
                    builder.Append(childText);
                    builder.Append("`");
                    break;

                default:
                    builder.Append(childText);
                    break;
            }
        }

        var result = builder.ToString(); //.Trim();

        return result;
    }

    private string ProcessChildren(XmlNode node)
    {
        var builder = new StringBuilder();

        for (var i = 0; i < node.ChildNodes.Count; i++)
        {
            var child = node.ChildNodes[i];

            if (child != null)
            {
                var childValue = ProcessNode(child); //.Trim();
                // builder.Append(' ');
                builder.Append(childValue);
            }
        }

        var result = builder.ToString();

        return result;
    }
}