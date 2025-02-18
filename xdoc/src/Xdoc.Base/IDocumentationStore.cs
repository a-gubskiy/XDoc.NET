using System.Reflection;

namespace Xdoc;

/// <summary>
/// Defines a contract for retrieving documentation comments for types and properties.
/// </summary>
public interface IDocumentationStore
{
    /// <summary>
    /// Get comment for the specified type
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    string GetCommentForType(Type type);

    /// <summary>
    /// Get comment for the specified property of the specified type
    /// </summary>
    /// <param name="type"></param>
    /// <param name="propertyName"></param>
    /// <returns></returns>
    string GetCommentForProperty(Type type, string propertyName);
    
    /// <summary>
    /// comment for the specified property
    /// </summary>
    /// <param name="propertyInfo"></param>
    /// <returns></returns>
    string GetCommentForProperty(PropertyInfo propertyInfo);
}
