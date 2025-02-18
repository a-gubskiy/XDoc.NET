namespace Xdoc;

public interface IXmlDocumentation
{
    /// <summary>
    /// Get comment for the specified type
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    string GetTypeComment(Type type);

    /// <summary>
    /// Get comment for the specified property of the specified type
    /// </summary>
    /// <param name="type"></param>
    /// <param name="propertyName"></param>
    /// <returns></returns>
    string GetPropertyComment(Type type, string propertyName);
}
