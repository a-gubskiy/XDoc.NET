using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace BitzArt.XDoc;

public static class IMutableEntityTypeExtensions
{
    public static void ConfigureEntity(this IMutableEntityType entityType, XDoc xDoc)
    {
        entityType.ConfigureEntityTypeComment(xDoc);

        var properties = entityType.GetProperties();

        foreach (var property in properties)
        {
            entityType.ConfigureEntityPropertyComment(xDoc, property);
        }
    }

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