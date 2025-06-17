using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace BitzArt.XDoc;

/// <summary>
/// Extensions for configuring XML documentation comments for Entity Framework Core entities.
/// </summary>
[PublicAPI]
public static class ModelBuilderExtensions
{
    /// <summary>
    /// Configures XML documentation comments for Entity Framework Core entities and their properties.
    /// </summary>
    /// <param name="modelBuilder"></param>
    /// <param name="xDoc"></param>
    public static ModelBuilder ConfigureComments(this ModelBuilder modelBuilder, XDoc xDoc)
    {
        var entityTypes = modelBuilder.Model.GetEntityTypes();

        foreach (var entityType in entityTypes)
        {
            ConfigureEntity(xDoc, entityType);
        }

        return modelBuilder;
    }

    private static void ConfigureEntity(XDoc xDoc, IMutableEntityType entityType)
    {
        ConfigureEntityTypeComment(xDoc, entityType);

        var properties = entityType.GetProperties();

        foreach (var property in properties)
        {
            ConfigureEntityPropertyComment(xDoc, entityType, property);
        }
    }

    private static void ConfigureEntityTypeComment(XDoc xDoc, IMutableEntityType entityType)
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
    
    private static void ConfigureEntityPropertyComment(XDoc xDoc, IMutableEntityType entityType, IMutableProperty property)
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