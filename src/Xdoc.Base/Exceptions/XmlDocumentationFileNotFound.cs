namespace Xdoc.Exceptions;

/// <summary>
/// The exception that is thrown when an XML documentation file is not found.
/// </summary>
public class XmlDocumentationFileNotFound : FileNotFoundException
{
    public string Path { get; }

    public XmlDocumentationFileNotFound(string message, string path)
        : base(message)
    {
        Path = path;
    }
}