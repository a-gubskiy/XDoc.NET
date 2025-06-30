using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BitzArt.XDoc;

/// <summary>
/// Provides extension methods for <see cref="OwnedNavigationBuilder{TOwnerEntity, TDependentEntity}"/> to map XML comments.
/// </summary>
public static class OwnedNavigationBuilderExtensions
{
    /// <summary>
    /// Maps XML comments from an XDoc instance to an entity type and its properties.
    /// </summary>
    /// <typeparam name="TOwnerEntity">The type of the owner entity.</typeparam>
    /// <typeparam name="TDependentEntity">The type of the dependent entity.</typeparam>
    /// <param name="builder">The owned navigation builder to extend.</param>
    /// <param name="xdoc">The XDoc instance containing XML comments to be mapped.</param>
    /// <returns>The same builder instance so that multiple calls can be chained.</returns>
    public static OwnedNavigationBuilder<TOwnerEntity, TDependentEntity> MapXmlComments<TOwnerEntity, TDependentEntity>(
        this OwnedNavigationBuilder<TOwnerEntity, TDependentEntity> builder, XDoc xdoc)
        where TOwnerEntity : class
        where TDependentEntity : class
    {
        var entityType = builder.Metadata.DeclaringEntityType;

        entityType.ConfigureEntityTypeComment(xdoc);

        var properties = entityType.GetProperties();

        foreach (var property in properties)
        {
            entityType.ConfigureEntityPropertyComment(xdoc, property);
        }

        return builder;
    }

}