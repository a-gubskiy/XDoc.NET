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

        // Split the input into lines and process
        var lines = input.Split('\n')
            .Select(line => line.TrimEnd())
            .ToArray();

        // Remove blank lines at the start
        int startIndex = 0;
        while (startIndex < lines.Length && string.IsNullOrWhiteSpace(lines[startIndex]))
        {
            startIndex++;
        }

        // Remove blank lines at the end
        int endIndex = lines.Length - 1;
        while (endIndex >= 0 && string.IsNullOrWhiteSpace(lines[endIndex]))
        {
            endIndex--;
        }

        // Ensure we don't have an invalid range
        if (startIndex > endIndex)
        {
            return string.Empty;
        }

        var result = new StringBuilder();
        bool lastLineWasEmpty = false;

        for (int i = startIndex; i <= endIndex; i++)
        {
            bool currentLineIsEmpty = string.IsNullOrWhiteSpace(lines[i]);
            
            // Skip consecutive empty lines
            if (currentLineIsEmpty && lastLineWasEmpty)
            {
                continue;
            }
            
            result.AppendLine(lines[i]);
            lastLineWasEmpty = currentLineIsEmpty;
        }

        return result.ToString().TrimEnd();
    }

    private string ProcessNode(XmlNode node)
    {
        var builder = new StringBuilder();

        if (node is XmlText textNode)
        {
            // Preserve whitespace in text nodes but trim excessive whitespace
            var text = textNode.Value;
            if (!string.IsNullOrWhiteSpace(text))
            {
                // Manually normalize whitespace without regex
                StringBuilder cleanedText = new StringBuilder();
                bool lastWasWhitespace = false;
                
                foreach (char c in text)
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
                // Add the whitespace directly for empty/whitespace text nodes
                builder.Append(text);
            }
        }
        else if (node is XmlElement element)
        {
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
                            value = value.Substring(2);
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

                case "para":
                    builder.AppendLine();
                    builder.AppendLine();
                    builder.Append(ProcessChildren(element).Trim());
                    builder.AppendLine();
                    builder.AppendLine();
                    break;

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
                        if (child.Name == "item")
                        {
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
                    }
                    builder.AppendLine();
                    break;

                case "code":
                    builder.AppendLine();
                    builder.AppendLine();
                    builder.AppendLine("```");
                    
                    // Get the original XML content to preserve formatting
                    string codeContent = element.InnerText ?? "";
                    
                    // Trim only leading/trailing whitespace, preserve internal structure
                    builder.Append(codeContent.Trim());
                    
                    builder.AppendLine();
                    builder.AppendLine("```");
                    builder.AppendLine();
                    break;

                case "example":
                    builder.AppendLine();
                    builder.AppendLine();
                    builder.Append(ProcessChildren(element).Trim());
                    builder.AppendLine();
                    break;

                case "c":
                    builder.Append("`" + ProcessChildren(element).Trim() + "`");
                    break;

                default:
                    builder.Append(ProcessChildren(element).Trim());
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