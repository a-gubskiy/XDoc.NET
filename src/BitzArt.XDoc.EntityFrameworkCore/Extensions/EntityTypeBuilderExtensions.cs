using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BitzArt.XDoc.EntityFrameworkCore;

/// <summary>
/// Extension methods for EntityTypeBuilder.
/// </summary>
public static class EntityTypeBuilderExtensions
{
    /// <summary>
    /// Add comment to the table with the same name as the entity.
    /// </summary>
    /// <param name="builder"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static EntityTypeBuilder<T> AddAutoComment<T>(this EntityTypeBuilder<T> builder)
        where T : class
    {
        return builder;
    }

    /// <summary>
    /// Add comment to all columns in the table.
    /// </summary>
    /// <param name="builder"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static EntityTypeBuilder<T> AddAutoCommentToAllColumns<T>(this EntityTypeBuilder<T> builder)
        where T : class
    {
        return builder;
    }
}