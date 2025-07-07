using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BitzArt.XDoc;

/// <summary>
/// Provides extension methods for <see cref="OwnedNavigationBuilder{TOwnerEntity, TDependentEntity}"/> to map XML comments.
/// </summary>
public static class OwnedNavigationBuilderExtensions
{
    /// <summary>
    /// Configures XML documentation for the entity type represented by the owned navigation builder.
    /// </summary>
    /// <typeparam name="TOwnerEntity">The type of the owner entity.</typeparam>
    /// <typeparam name="TDependentEntity">The type of the dependent entity.</typeparam>
    /// <param name="builder">The owned navigation builder to configure.</param>
    /// <param name="xdoc">The XML documentation container.</param>
    /// <returns>The same builder instance so that multiple configuration calls can be chained.</returns>
    public static OwnedNavigationBuilder<TOwnerEntity, TDependentEntity> ConfigureEntityXmlDocumentation<TOwnerEntity,
        TDependentEntity>(
        this OwnedNavigationBuilder<TOwnerEntity, TDependentEntity> builder,
        XDoc xdoc)
        where TOwnerEntity : class
        where TDependentEntity : class
    {
        var entityType = builder.OwnedEntityType;

        entityType.ConfigureEntityXmlDocumentation(xdoc);

        return builder;
    }

    /// <summary>
    /// Configures XML documentation for all properties of the entity type represented by the owned navigation builder.
    /// </summary>
    /// <typeparam name="TOwnerEntity">The type of the owner entity.</typeparam>
    /// <typeparam name="TDependentEntity">The type of the dependent entity.</typeparam>
    /// <param name="builder">The owned navigation builder to configure.</param>
    /// <param name="xdoc">The XML documentation container.</param>
    /// <returns>The same builder instance so that multiple configuration calls can be chained.</returns>
    public static OwnedNavigationBuilder<TOwnerEntity, TDependentEntity> ConfigurePropertiesXmlDocumentation<
        TOwnerEntity, TDependentEntity>(
        this OwnedNavigationBuilder<TOwnerEntity, TDependentEntity> builder,
        XDoc xdoc)
        where TOwnerEntity : class
        where TDependentEntity : class
    {
        var entityType = builder.Metadata.DeclaringEntityType;

        var properties = entityType.GetProperties();

        foreach (var property in properties)
        {
            entityType.ConfigurePropertyXmlDocumentation(xdoc, property);
        }

        return builder;
    }
}