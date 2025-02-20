namespace BitzArt.XDoc;

/// <summary>
/// Represents an exception that is thrown
/// when the XML documentation file
/// for a requested assembly is not found.
/// </summary>
/// <param name="message">Exception message.</param>
public class XmlDocumentationFileNotFoundException(
    string message = XmlDocumentationFileNotFoundException.DefaultMessage)
    : FileNotFoundException(message)
{
    internal const string DefaultMessage = "XML documentation file not found for the specified assembly."
        + "Ensure that the XML documentation file is generated and included in the output directory.";

    internal static void ThrowIfFileNotFound(string filePath)
    {
        if (!File.Exists(filePath)) throw new XmlDocumentationFileNotFoundException();
    }
}