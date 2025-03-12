using System.Reflection;
using System.Xml;

namespace BitzArt.XDoc;

internal static class XmlUtility
{
    /// <summary>
    /// Fetches XML documentation for the specified <see cref="Assembly"/>.<br/>
    /// </summary>
    /// <returns>
    /// A Dictionary containing all assembly types as keys and their respective <see cref="TypeDocumentation"/> as values, if any.<br/>
    /// In case no documentation is found for a type, the respective value will be <see langword="null"/>.
    /// </returns>
    internal static Dictionary<Type, TypeDocumentation> Fetch(XDoc source, Assembly assembly)
    {
        try
        {
            var filePath = GetXmlDocumentationFilePath(assembly);

            if (string.IsNullOrEmpty(filePath))
            {
                return new Dictionary<Type, TypeDocumentation>();
            }

            // 'using' statement ensures the file stream is disposed of after use
            using var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);

            var xmlDocument = new XmlDocument();
            xmlDocument.Load(fileStream);

            return XmlParser.Parse(source, assembly, xmlDocument);
        }
        catch (Exception ex)
        {
            throw new XDocException("Something went wrong while trying to parse the XML documentation file. " +
                                    "See inner exception for details.", ex);
        }
    }

    public static string GetXmlDocumentationFilePath(Assembly assembly)
    {
        // Try to find local XML documentation file
        var assemblyLocation = assembly.Location;
        var localXmlPath = Path.ChangeExtension(assemblyLocation, "xml");
        
        if (File.Exists(localXmlPath))
        {
            return localXmlPath;
        }

        return string.Empty;
    }
}