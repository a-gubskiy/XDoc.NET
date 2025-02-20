namespace Xdoc;

public class XmlDocumentationFileNotFound : FileNotFoundException
{
    public string Path { get; }

    public XmlDocumentationFileNotFound(string message, string path)
        : base(message)
    {
        Path = path;
    }
}