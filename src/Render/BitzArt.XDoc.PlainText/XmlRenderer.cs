using System.Text;
using System.Xml;

namespace Xdoc.Renderer.PlainText;

/// <summary>
/// Renders XML documentation to plain text.
/// <seealso href="https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/xmldoc/xml-documentation-comments">Microsoft specification</seealso>
/// </summary>
public class XmlRenderer
{
    
    /// <summary>
    /// Converts an XML documentation node to the plain text.
    /// </summary>
    /// <param name="xmlNode">The XML node to render.</param>
    /// <returns>A plain text string representation of the XML node, or an empty string if the node is null.</returns>
    public string Render(XmlNode? xmlNode)
    {
        if (xmlNode == null)
        {
            return string.Empty;
        }

        var text = ProcessNode(xmlNode);
        var normalizedText = Normalize(text);
        
        return normalizedText;
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

        // Split the input into lines and process
        var lines = input.Split('\n')
            .Select(line => line.TrimEnd())
            .ToList();

        var startIndex = GetStartIndex(lines);
        var endIndex = GetEndIndex(lines);

        if (startIndex > endIndex)
        {
            throw new ArgumentException("Invalid text range: start index cannot be greater than end index.", nameof(input));
        }

        var builder = new StringBuilder();
        
        var lastLineWasEmpty = false;

        for (var i = startIndex; i <= endIndex; i++)
        {
            var line = lines[i];
            
            var currentLineIsEmpty = string.IsNullOrWhiteSpace(line);
            
            if (currentLineIsEmpty && lastLineWasEmpty)
            {
                continue;
            }
            
            builder.AppendLine(line);
            
            lastLineWasEmpty = currentLineIsEmpty;
        }

        var result = builder.ToString().TrimEnd();
        
        return result;
    }

    /// <summary>
    /// Gets the index of the first non-empty line in the list of strings.
    /// </summary>
    /// <param name="lines">The list of strings to process.</param>
    /// <returns>The index of the first non-empty line, or the list count if all lines are empty.</returns>
    /// <example>
    /// For input ["", "text", "", ""], returns 1
    /// For input ["", "", ""], returns 3
    /// </example>
    private static int GetStartIndex(IReadOnlyCollection<string> lines)
    {
        var startIndex = 0;
        var line = lines.ElementAt(startIndex);
        
        while (startIndex < lines.Count && string.IsNullOrWhiteSpace(line))
        {
            startIndex++;
        }

        return startIndex;
    }

    /// <summary>
    /// Gets the index of the last non-empty line in the list of strings.
    /// </summary>
    /// <param name="lines">The list of strings to process.</param>
    /// <returns>The index of the last non-empty line, or -1 if all lines are empty or the list is empty.</returns>
    /// <example>
    /// For input ["", "text", "", ""], returns 1
    /// For input ["", "", ""], returns -1
    /// </example>
    private static int GetEndIndex(IReadOnlyCollection<string> lines)
    {
        var endIndex = lines.Count - 1;
        var line = lines.ElementAt(endIndex);
        
        while (endIndex >= 0 && string.IsNullOrWhiteSpace(line))
        {
            endIndex--;
        }

        return endIndex;
    }

    /// <summary>
    /// Processes an XML node and its children, converting them to their plain text representation.
    /// </summary>
    /// <param name="node">The XML node to process. Can be a text node or an XML element.</param>
    /// <returns>A string containing the plain text representation of the node and its children.</returns>
    private string ProcessNode(XmlNode node)
    {
        var builder = new StringBuilder();

        if (node is XmlText textNode)
        {
            builder.Append(RenderTextNode(textNode));
        }
        else if (node is XmlElement element)
        {
            builder.Append(RenderXmlElement(element));
        }

        var result = builder.ToString();
        
        return result;
    }

    private string RenderXmlElement(XmlElement element)
    {
        var builder = new StringBuilder();
            
        var crefAttribute = element.Attributes["cref"];
        var nameAttribute = element.Attributes["name"];
        var elementAttribute = element.Attributes["type"];
            
        switch (element.Name)
        {
            case "see":
            case "seealso":
                if (crefAttribute != null)
                {
                    var crefValue = crefAttribute.Value;
                    if (crefValue.StartsWith("T:"))
                    {
                        crefValue = crefValue[2..];
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
                builder.AppendLine();
                builder.Append("(Parameter: " + nameAttribute?.Value + ") ");
                builder.Append(ProcessChildren(element).Trim());
                builder.AppendLine();
                break;

            case "returns":
                builder.AppendLine();
                builder.Append("Returns: " + ProcessChildren(element).Trim());
                builder.AppendLine();
                break;

            case "exception":
                builder.AppendLine();
                builder.Append("Throws ");
                    
                if (crefAttribute != null)
                {
                    var value = crefAttribute.Value;
                        
                    if (value.StartsWith("T:"))
                    {
                        value = value[2..];
                    }
                        
                    builder.Append(value);
                }
                    
                builder.Append(": " + ProcessChildren(element).Trim());
                builder.AppendLine();
                break;

            case "value":
                builder.AppendLine();
                builder.Append("Value: " + ProcessChildren(element).Trim());
                builder.AppendLine();
                break;
            
            case "example":
            case "para":
            case "remarks":
                builder.AppendLine();
                builder.AppendLine();
                builder.Append(ProcessChildren(element).Trim());
                builder.AppendLine();
                break;
 
            case "list":
                builder.AppendLine();
                builder.AppendLine();
                    
                var listType = elementAttribute?.Value;
                    
                foreach (XmlNode child in element.ChildNodes)
                {
                    if (child.Name != "item")
                    {
                        continue;
                    }
                        
                    var itemText = ProcessChildren(child).Trim();
                            
                    var prefix = listType switch
                    {
                        "number" => "1. ",
                        "bullet" => "– ",
                        "table" => "* ",
                        _ => "– "
                    };
                            
                    builder.AppendLine(prefix + itemText);
                }
                    
                builder.AppendLine();
                break;

            case "code":
                builder.AppendLine();
                builder.AppendLine();
                builder.AppendLine("```");
                    
                var codeContent = element.InnerText ?? "";
                    
                builder.Append(codeContent);
                    
                builder.AppendLine();
                builder.AppendLine("```");
                builder.AppendLine();
                break;

   

            case "c":
                builder.Append("`" + ProcessChildren(element).Trim() + "`");
                break;

            default:
                builder.Append(ProcessChildren(element).Trim());
                break;
        }

        var result = builder.ToString();
        
        return result;
    }

    private static string RenderTextNode(XmlText textNode)
    {
        var builder = new StringBuilder(); 

        if (!string.IsNullOrWhiteSpace(textNode.Value))
        {
            var cleanedText = new StringBuilder();
            var lastWasWhitespace = false;
                
            foreach (var c in textNode.Value)
            {
                if (char.IsWhiteSpace(c))
                {
                    if (!lastWasWhitespace)
                    {
                        cleanedText.Append(' ');
                        lastWasWhitespace = true;
                    }
                }
                else
                {
                    cleanedText.Append(c);
                    lastWasWhitespace = false;
                }
            }
                
            builder.Append(cleanedText.ToString());
        }
        else
        {
            builder.Append(textNode.Value);
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

        var result = builder.ToString();
        
        return result;
    }
}