using System.Linq.Expressions;
using BitzArt.XDoc.PlainText;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BitzArt.XDoc.EntityFrameworkCore;

/// <summary>
/// Extension methods for PropertyBuilder.
/// </summary>
[PublicAPI]
public static class PropertyBuilderExtensions
{
    private static readonly XDoc XDoc = new XDoc();
    
    /// <summary>
    /// Map property comment from XML documentation to the property.
    /// </summary>
    /// <param name="builder">
    /// Property builder.
    /// </param>
    /// <param name="propertyExpression">
    /// Expression to get the property name.
    /// </param>
    /// <typeparam name="TProperty">
    /// Property type.
    /// </typeparam>
    /// <typeparam name="TMappedPropertyEntity">
    /// Entity type where the property is defined.
    /// </typeparam>
    /// <returns></returns>
    public static PropertyBuilder<TProperty> MapPropertyComment<TProperty, TMappedPropertyEntity>(
        this PropertyBuilder<TProperty> builder,
        Expression<Func<TMappedPropertyEntity, object?>> propertyExpression)
    {
        var type = typeof(TMappedPropertyEntity);
        
        var expression = propertyExpression.Body as UnaryExpression;
        var operand = expression?.Operand as MemberExpression;
        var propertyName = operand?.Member.Name;

        if (string.IsNullOrWhiteSpace(propertyName))
        {
            return builder;
        }

        var propertyInfo = type.GetProperty(propertyName);

        var comment = XDoc.Get(propertyInfo).ToPlainText();

        if (!string.IsNullOrWhiteSpace(comment))
        {
            builder.HasComment(comment);
        }

        return builder;
    }
}