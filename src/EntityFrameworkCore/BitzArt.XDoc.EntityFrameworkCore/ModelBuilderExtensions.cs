using Microsoft.EntityFrameworkCore;

namespace BitzArt.XDoc;

/// <summary>
/// Extensions for configuring XML documentation comments for Entity Framework Core entities.
/// </summary>
public static class ModelBuilderExtensions
{
    /// <summary>
    /// Configures XML documentation comments for Entity Framework Core entities and their properties.
    /// </summary>
    /// <param name="modelBuilder"></param>
    /// <param name="xDoc"></param>
    public static void ConfigureComments(this ModelBuilder modelBuilder, XDoc xDoc)
    {
        var entityTypes = modelBuilder.Model.GetEntityTypes();

        foreach (var entityType in entityTypes)
        {
            var typeDocumentation = xDoc.Get(entityType.ClrType);

            if (typeDocumentation is null)
            {
                continue;
            }

            var entityComment = typeDocumentation.ToPlainText();

            // For owned entities, we don't set the comment on the entity itself
            // But we will set the comment on the properties

            var isOwned = entityType.IsOwned();
            var tableName = entityType.GetTableName();

            if (!isOwned && tableName is not null)
            {
                entityType.SetComment(entityComment);
            }

            var properties = entityType.GetProperties();

            foreach (var property in properties)
            {
                var isShadowProperty = property.IsShadowProperty();

                if (isShadowProperty)
                {
                    continue;
                }

                var propertyInfo = entityType.ClrType.GetProperty(property.Name);

                if (propertyInfo is null)
                {
                    continue;
                }

                var propertyDocumentation = xDoc.Get(propertyInfo);

                if (propertyDocumentation is null)
                {
                    continue;
                }

                var propertyComment = propertyDocumentation.ToPlainText();

                property.SetComment(propertyComment);
            }
        }
    }
}