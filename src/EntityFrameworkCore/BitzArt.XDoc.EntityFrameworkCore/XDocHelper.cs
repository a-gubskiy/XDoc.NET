using System.Linq.Expressions;

namespace BitzArt.XDoc;

public static class XDocHelper
{
    /// <summary>
    /// Extracts documentation comment from a property specified in an expression.
    /// </summary>
    /// <typeparam name="TEntity">The entity type being configured</typeparam>
    /// <typeparam name="TCommentTargetEntity">The entity type containing the property whose documentation will be used</typeparam>
    /// <typeparam name="TCommentTargetProperty">The type of the property whose documentation will be used</typeparam>
    /// <param name="xdoc">The XDoc instance used to extract documentation</param>
    /// <param name="commentTargetPropertyExpression">An expression that identifies the property whose documentation will be used</param>
    /// <returns>The extracted documentation comment as plain text</returns>
    /// <exception cref="InvalidOperationException">Thrown when the property name cannot be determined or is empty</exception>
    internal static string GetComment<TCommentTargetEntity, TCommentTargetProperty>(
        XDoc xdoc,
        Expression<Func<TCommentTargetEntity, TCommentTargetProperty>> commentTargetPropertyExpression)
        where TCommentTargetEntity : class
    {
        var operand = commentTargetPropertyExpression.Body as MemberExpression;
        var targetPropertyName = operand!.Member.Name;

        if (string.IsNullOrWhiteSpace(targetPropertyName))
        {
            throw new InvalidOperationException($"Property with name '{targetPropertyName}' was not found.");
        }

        var comment = GetComment<TCommentTargetEntity>(xdoc, targetPropertyName);

        return comment;
    }

    /// <summary>
    /// Extracts documentation comment from a property with the specified name in the given entity type.
    /// </summary>
    /// <typeparam name="TCommentTargetEntity">The entity type containing the property whose documentation will be used</typeparam>
    /// <param name="xdoc">The XDoc instance used to extract documentation</param>
    /// <param name="propertyName">The name of the property whose documentation will be extracted</param>
    /// <returns>The extracted documentation comment as plain text</returns>
    internal static string GetComment<TCommentTargetEntity>(XDoc xdoc, string propertyName)
    {
        var propertyInfo = typeof(TCommentTargetEntity).GetProperty(propertyName);

        var comment = xdoc.Get(propertyInfo).ToPlainText();

        return comment;
    }
}