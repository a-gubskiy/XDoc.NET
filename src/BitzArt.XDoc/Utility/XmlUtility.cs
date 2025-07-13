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
        var filePath = GetXmlDocumentationFilePath(assembly);

        if (string.IsNullOrEmpty(filePath))
        {
            return [];
        }

        // 'using' statement ensures the file stream is disposed of after use
        using var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);

        var xmlDocument = new XmlDocument();
        xmlDocument.Load(fileStream);

        return Fetch(xmlDocument, source, assembly);
    }

    /// <summary>
    /// Fetches XML documentation for the specified <see cref="Assembly"/>.<br/>
    /// </summary>
    /// <returns>
    /// A Dictionary containing all assembly types as keys and their respective <see cref="TypeDocumentation"/> as values, if any.<br/>
    /// In case no documentation is found for a type, the respective value will be <see langword="null"/>.
    /// </returns>
    internal static Dictionary<Type, TypeDocumentation> Fetch(XmlDocument xmlDocument, XDoc source, Assembly assembly)
    {
        try
        {
            var typesDocumentation = XmlParser.Parse(source, assembly, xmlDocument);

            return typesDocumentation;
        }
        catch (Exception ex)
        {
            throw new AggregateException("Something went wrong while trying to parse the XML documentation file. " +
                                    "See inner exception for details.", ex);
        }
    }

    private static string GetXmlDocumentationFilePath(Assembly assembly)
    {
        // Try to find local XML documentation file
        
        // Note: At runtime, .NET loads assemblies from the output directory.
        // But during EF Core migrations, assemblies may be loaded from the NuGet cache.
        // This method always looks for the XML doc in the output folder (AppContext.BaseDirectory),
        // so ensure the XML file is present there for both runtime and migrations.

        var fileName = Path.GetFileName(assembly.Location);
        var localXmlPath = Path.ChangeExtension(Path.Combine(AppContext.BaseDirectory, fileName), "xml");
        
        if (File.Exists(localXmlPath))
        {
            return localXmlPath;
        }

        return string.Empty;
    }
}