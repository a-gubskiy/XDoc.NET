using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace BitzArt.XDoc;

/// <summary>
/// Extension methods for <see cref="IMutableEntityType"/> that add XML documentation as comments
/// to database entities and their properties.
/// </summary>
public static class MutableEntityTypeExtensions
{
    /// <summary>
    /// Configures an entity and all its properties with comments from XML documentation.
    /// </summary>
    /// <param name="entityType">The entity type to configure.</param>
    /// <param name="xDoc">The XDoc instance containing XML documentation.</param>
    public static void ConfigureEntityComments(this IMutableEntityType entityType, XDoc xDoc)
    {
        entityType.ConfigureEntityTypeComment(xDoc);

        var properties = entityType.GetProperties();

        foreach (var property in properties)
        {
            entityType.ConfigureEntityPropertyComment(xDoc, property);
        }
    }

    /// <summary>
    /// Configures an entity type with a comment from its XML documentation.
    /// </summary>
    /// <param name="entityType">The entity type to configure.</param>
    /// <param name="xDoc">The XDoc instance containing XML documentation.</param>
    /// <remarks>
    /// Comments are not applied to owned entities or entities without a table name.
    /// </remarks>
    public static void ConfigureEntityTypeComment(this IMutableEntityType entityType, XDoc xDoc)
    {
        var typeDocumentation = xDoc.Get(entityType.ClrType);

        if (typeDocumentation is null)
        {
            // No own xml-documentation
            return;
        }

        var isOwned = entityType.IsOwned();
        var tableName = entityType.GetTableName();

        if (isOwned || tableName is null)
        {
            // For owned entities, we don't set the comment on the entity itself
            // But we will set the comment on the properties

            return;
        }

        var entityComment = typeDocumentation.ToPlainText();

        entityType.SetComment(entityComment);
    }

    /// <summary>
    /// Configures an entity property with a comment from its XML documentation.
    /// </summary>
    /// <param name="entityType">The entity type containing the property.</param>
    /// <param name="xDoc">The XDoc instance containing XML documentation.</param>
    /// <param name="property">The property to configure.</param>
    /// <remarks>
    /// Comments are not applied to shadow properties or properties without documentation.
    /// </remarks>
    public static void ConfigureEntityPropertyComment(this IMutableEntityType entityType, XDoc xDoc, IMutableProperty property)
    {
        var isShadowProperty = property.IsShadowProperty();

        if (isShadowProperty)
        {
            return;
        }

        var propertyInfo = entityType.ClrType.GetProperty(property.Name);

        if (propertyInfo is null)
        {
            return;
        }

        var propertyDocumentation = xDoc.Get(propertyInfo);

        if (propertyDocumentation is null)
        {
            return;
        }

        var propertyComment = propertyDocumentation.ToPlainText();

        property.SetComment(propertyComment);
    }
}