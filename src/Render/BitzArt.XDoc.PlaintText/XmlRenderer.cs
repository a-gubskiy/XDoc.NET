using System.Xml;
using System;
using System.Text;
using System.Xml;

namespace Xdoc.Renderer.PlaintText;

public class XmlRenderer
{
    public string Render(XmlNode? xmlNode)
    {
        if (xmlNode == null)
        {
            return string.Empty;
        }


        var text = Normalize(ProcessNodes(xmlNode));

        return text;
    }

    /// <summary>
    /// Normalize the input string by removing empty lines and trimming the lines.
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
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

        var result = string.Join("\n", lines);

        return result;
    }


    private string ProcessNodes(XmlNode node)
    {
        var builder = new StringBuilder();

        if (node is XmlText textNode)
        {
            builder.Append(textNode.Value);
        }
        else if (node is XmlElement element)
        {
            var attribute = element.Attributes["cref"];

            if (element.Name == "see" && attribute != null)
            {
                var crefValue = attribute.Value;
                var className = "";

                if (crefValue.Split('.').Length > 0)
                {
                    className = crefValue.Split('.')[^1];
                }
                else
                {
                    className = crefValue;
                }

                builder.Append(className);
            }
            else
            {
                foreach (XmlNode child in element.ChildNodes)
                {
                    builder.Append(ProcessNodes(child));
                }
            }
        }

        return builder.ToString();
    }
}