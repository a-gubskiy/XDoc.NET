using System.Reflection;
using System.Runtime.InteropServices;
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
        // 1. Try to find local XML documentation file
        var assemblyLocation = assembly.Location;
        var localXmlPath = Path.ChangeExtension(assemblyLocation, "xml");
        
        if (File.Exists(localXmlPath))
        {
            return localXmlPath;
        }
    
        // 2. For framework assemblies
        if (assembly.FullName?.StartsWith("System") == true ||
            assembly.FullName?.StartsWith("mscorlib") == true)
        {
            var systemXmlDocumentationFilePath = GetSystemXmlDocumentationFilePath(assembly);
            
            if (File.Exists(systemXmlDocumentationFilePath))
            {
                return systemXmlDocumentationFilePath;
            }
        }
    
        throw new FileNotFoundException($"Could not find XML documentation file for {assembly.FullName}", Path.ChangeExtension(assembly.Location, "xml"));
    }

    private static string GetSystemXmlDocumentationFilePath(Assembly assembly)
    {
        string assemblyName = assembly.GetName().Name!;
            
        // Get runtime directory
        var runtimeDirectory = RuntimeEnvironment.GetRuntimeDirectory();
            
        // Get .NET runtime version (e.g., "8.0.12")
        var runtimeVersion = RuntimeEnvironment.GetSystemVersion().Replace("v", "");
            
        // Find the .NET SDK root
        var dotnetRoot = Path.GetFullPath(Path.Combine(runtimeDirectory, "..", "..", ".."));
            
        // Get the major version of the .NET runtime
        var majorDotnetVersion = runtimeVersion.Split('.')[0];

        var dotnetVersionDirectoryName = "net" + majorDotnetVersion + ".0";
            
        // Common locations for documentation files
        List<string> searchPaths =
        [
            Path.Combine(runtimeDirectory, $"{assemblyName}.xml"),
            Path.Combine(runtimeDirectory, "System.Runtime.xml"),

            // Check in packs folder
            Path.Combine(dotnetRoot, "packs", "Microsoft.NETCore.App.Ref", runtimeVersion, "ref",
                dotnetVersionDirectoryName, $"{assemblyName}.xml"),
                
            Path.Combine(dotnetRoot, "packs", "Microsoft.NETCore.App.Ref", runtimeVersion, "ref",
                dotnetVersionDirectoryName, "System.Runtime.xml"),

            // For macOS-specific paths
            Path.Combine("/usr/local/share/dotnet/packs/Microsoft.NETCore.App.Ref", runtimeVersion, "ref",
                dotnetVersionDirectoryName, "System.Runtime.xml"),

            // For Windows-specific paths
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "dotnet", "packs",
                "Microsoft.NETCore.App.Ref", runtimeVersion, "ref", dotnetVersionDirectoryName,
                "System.Runtime.xml")
        ];
    
        foreach (var path in searchPaths)
        {
            if (File.Exists(path))
            {
                return path;
            }
        }
    
        // If specific version not found, try to find any version by scanning directories
        var packsPath = Path.Combine(dotnetRoot, "packs", "Microsoft.NETCore.App.Ref");
            
        if (Directory.Exists(packsPath))
        {
            var versions = Directory.GetDirectories(packsPath).OrderByDescending(v => v);
            
            foreach (var version in versions)
            {
                string targetFramework = "net" + Path.GetFileName(version).Split('.')[0] + ".0";
                string xmlPath = Path.Combine(version, "ref", targetFramework, "System.Runtime.xml");
                
                if (File.Exists(xmlPath))
                {
                    return xmlPath;
                }
            }
        }

        return string.Empty;
    }

    private static string? GetAssemblyName(Assembly assembly)
    {
        var name = assembly.GetName();

        return name.Name;
    }
}