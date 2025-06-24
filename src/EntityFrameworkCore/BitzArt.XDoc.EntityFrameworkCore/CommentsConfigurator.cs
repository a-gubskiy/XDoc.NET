using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace BitzArt.XDoc;

/// <summary>
/// Configures database comments for Entity Framework Core models based on XML documentation.
/// </summary>
public class CommentsConfigurator
{
    /// <summary>
    /// The XML documentation provider
    /// </summary>
    private readonly XDoc _xDoc;

    /// <summary>
    /// List of processed database columns to prevent comments conflicts.
    /// </summary>
    private readonly HashSet<string> _processedColumns = [];

    /// <summary>
    /// Initializes a new instance of the <see cref="CommentsConfigurator"/> class.
    /// </summary>
    /// <param name="xDoc">The XML documentation provider.</param>
    public CommentsConfigurator(XDoc xDoc)
    {
        _xDoc = xDoc;
    }

    /// <summary>
    /// Sets comments on all entity types in the model.
    /// </summary>
    /// <param name="modelBuilder">The model builder containing entity type definitions.</param>
    public void SetComments(ModelBuilder modelBuilder)
    {
        var entityTypes = modelBuilder.Model.GetEntityTypes();

        foreach (var entityType in entityTypes)
        {
            SetComments(entityType);
        }
    }

    /// <summary>
    /// Sets comments on a specific entity type and all its properties.
    /// </summary>
    /// <param name="entityType">The entity type to set comments on.</param>
    public void SetComments(IMutableEntityType entityType)
    {
        SetEntityTypeComment(entityType);

        var properties = entityType.GetProperties();

        foreach (var property in properties)
        {
            SetEntityPropertyComment(entityType, property);
        }
    }

    /// <summary>
    /// Sets a comment on an entity type (database table)
    /// </summary>
    /// <param name="entityType">The entity type to set a comment on.</param>
    /// <remarks>
    /// Comments are not set for owned entities or entities without a table name.
    /// Also skips if the base type already defines the comment for the same table.
    /// </remarks>
    private void SetEntityTypeComment(IMutableEntityType entityType)
    {
        var typeDocumentation = _xDoc.Get(entityType.ClrType);

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

        if (entityType.BaseType != null && entityType.BaseType.GetTableName() == tableName)
        {
            // Base type already defined comment
            return;
        }

        var entityComment = typeDocumentation.ToPlainText();

        entityType.SetComment(entityComment);
    }

    /// <summary>
    /// Sets a comment on an entity property (database column)
    /// </summary>
    /// <param name="entityType">The entity type containing the property.</param>
    /// <param name="property">The property to set a comment on.</param>
    /// <remarks>
    /// Skips shadow properties, properties that have already been processed, 
    /// and properties without XML documentation.
    /// </remarks>
    private void SetEntityPropertyComment(IMutableEntityType entityType, IMutableProperty property)
    {
        var columnName = $"{entityType.GetTableName()}.{property.GetColumnName()}";
        
        
        if (!_processedColumns.Add(columnName))
        {
            // Column already processed, skip to avoid conflicts
            return;
        }

        var isShadowProperty = property.IsShadowProperty();

        if (isShadowProperty)
        {
            return;
        }

        var propertyInfo = entityType.ClrType.GetProperty(property.Name);

        if (propertyInfo is null)
        {
            // Property not found in the CLR type
            return;
        }

        var propertyDocumentation = _xDoc.Get(propertyInfo);

        if (propertyDocumentation is null)
        {
            // No own xml-documentation for the property
            return;
        }

        var propertyComment = propertyDocumentation.ToPlainText();

        property.SetComment(propertyComment);
    }
}