using System.Reflection;
using JetBrains.Annotations;

namespace Xdoc;

/// <summary>
/// Defines a contract for retrieving documentation comments for types and properties.
/// </summary>
[PublicAPI]
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

public abstract class DocumentationStoreBase : IDocumentationStore
{
    public abstract string GetCommentForType(Type type);

    public abstract string GetCommentForProperty(Type type, string propertyName);

    /// <inheritdoc />
    public virtual string GetCommentForProperty(PropertyInfo propertyInfo)
    {
        var type = propertyInfo.DeclaringType ?? propertyInfo.ReflectedType;
        var propertyName = propertyInfo.Name;
        
        if (type == null)
        {
            return string.Empty;
        }
        
        return GetCommentForProperty(type, propertyName);
    }
}