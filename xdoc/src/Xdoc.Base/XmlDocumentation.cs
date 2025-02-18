using System.Reflection;
using System.Xml;
using JetBrains.Annotations;

namespace Xdoc;

/// <summary>
/// Provides functionality to load and parse XML documentation files for assemblies.
/// </summary>
[PublicAPI]
public static class XmlDocumentation
{
    /// <summary>
    /// Load XML documentation from the specified path.
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public static XmlDocument Load(string path)
    {
        if (string.IsNullOrWhiteSpace(path))
        {
            throw new ArgumentException($"{nameof(path)} cannot be null or whitespace.");
        }

        var pathToXmlDocumentation = Path.ChangeExtension(path, "xml");

        if (File.Exists(pathToXmlDocumentation))
        {
            var xml = File.ReadAllText(pathToXmlDocumentation);

            var document = new XmlDocument();
            document.LoadXml(xml);

            return document;
        }

        throw new FileNotFoundException($"XML documentation file not found: {path}");
    }

    /// <summary>
    /// Load XML documentation for the specified assembly asynchronously.
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public static async Task<XmlDocument> LoadAsync(string path)
    {
        if (string.IsNullOrWhiteSpace(path))
        {
            throw new ArgumentException($"{nameof(path)} cannot be null or whitespace.");
        }

        var pathToXmlDocumentation = Path.ChangeExtension(path, "xml");

        if (File.Exists(pathToXmlDocumentation))
        {
            var xml = await File.ReadAllTextAsync(pathToXmlDocumentation);

            var document = new XmlDocument();
            document.LoadXml(xml);

            return document;
        }

        throw new FileNotFoundException($"XML documentation file not found: {path}");
    }

    /// <summary>
    /// Load XML documentation for the specified assembly.
    /// </summary>
    /// <param name="assembly"></param>
    /// <returns></returns>
    public static XmlDocument Load(Assembly assembly)
    {
        return Load(assembly.Location);
    }

    /// <summary>
    /// Load XML documentation from the specified path asynchronously.
    /// </summary>
    /// <param name="assembly"></param>
    /// <returns></returns>
    public static Task<XmlDocument> LoadAsync(Assembly assembly)
    {
        return LoadAsync(assembly.Location);
    }

    /// <summary>
    /// Load XML documentation for the current AppDomain.
    /// </summary>
    /// <returns></returns>
    public static IReadOnlyCollection<XmlDocument> LoadForCurrentAppDomain()
    {
        var result = new List<XmlDocument>();
        var assemblies = AppDomain.CurrentDomain.GetAssemblies();

        foreach (var assembly in assemblies)
        {
            var document = Load(assembly);

            result.Add(document);
        }

        return result;
    }

    /// <summary>
    /// Load XML documentation for the current AppDomain asynchronously.
    /// </summary>
    /// <returns></returns>
    public static async Task<IReadOnlyCollection<XmlDocument>> LoadForCurrentAppDomainAsync()
    {
        var result = new List<XmlDocument>();
        var assemblies = AppDomain.CurrentDomain.GetAssemblies();

        foreach (var assembly in assemblies)
        {
            var document = await LoadAsync(assembly);

            result.Add(document);
        }

        return result;
    }
}