using System.Linq.Expressions;

namespace BitzArt.XDoc;

internal static class XDocExtensions
{
    /// <summary>
    /// Fetches documentation for a property specified in an <see cref="Expression"/>.
    /// </summary>
    /// <typeparam name="TCommentTargetEntity">The entity type containing the property whose documentation will be used</typeparam>
    /// <typeparam name="TCommentTargetProperty">The type of the property whose documentation will be used</typeparam>
    /// <param name="xdoc">The XDoc instance used to extract documentation</param>
    /// <param name="commentTargetPropertyExpression">An expression that identifies the property whose documentation will be used</param>
    /// <returns>The extracted documentation comment as plain text</returns>
    /// <exception cref="InvalidOperationException">Thrown when the property name cannot be determined or is empty</exception>
    internal static PropertyDocumentation? Get<TCommentTargetEntity, TCommentTargetProperty>(
        this XDoc xdoc,
        Expression<Func<TCommentTargetEntity, TCommentTargetProperty>> commentTargetPropertyExpression)
        where TCommentTargetEntity : class
    {
        var operand = commentTargetPropertyExpression.Body as MemberExpression;
        var targetPropertyName = operand!.Member.Name;

        if (string.IsNullOrWhiteSpace(targetPropertyName))
        {
            throw new InvalidOperationException($"Property with name '{targetPropertyName}' was not found.");
        }
        
        var propertyInfo = typeof(TCommentTargetEntity).GetProperty(targetPropertyName);

        return xdoc.Get(propertyInfo);
    }
}