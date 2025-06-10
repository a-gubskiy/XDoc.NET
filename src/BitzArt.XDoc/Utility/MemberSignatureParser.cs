using System.Text.RegularExpressions;

namespace BitzArt.XDoc;

/// <summary>
/// Provides utility methods for parsing and resolving information from fully qualified member signatures,
/// such as extracting type names, member names, and method parameter types. 
/// Handles special cases including generic types and nested generic parameters.
/// </summary>
internal static class MemberSignatureParser
{
    /// <summary>
    /// Resolves a qualified member name into its associated type and member name.
    /// Handles special cases like generic types and methods with parameters.
    /// </summary>
    /// <param name="value">The fully qualified member name (e.g. "Namespace.TypeName.MemberName")</param>
    /// <returns>A tuple containing the resolved Type and the simple member name</returns>
    /// <exception cref="InvalidOperationException">
    /// Thrown when the name doesn't contain a type/member separator or when the type cannot be found
    /// </exception>
    public static (string typeName, string memberName) ResolveTypeAndMemberName(string value)
    {
        if (value.Contains('`'))
        {
            return ResolveGenericTypeAndMemberName(value);
        }

        if (value.Contains('('))
        {
            value = value[..value.IndexOf('(')];
        }

        var index = value.LastIndexOf('.');

        if (index == -1)
        {
            throw new InvalidOperationException("Encountered invalid XML node.");
        }

        var (typeName, memberName) = (value[..index], value[(index + 1)..]);


        return (typeName, memberName);
    }

    /// <summary>
    /// Resolves the type and member name from a generic type member signature.
    /// Handles cases where the member name includes generic type parameters or method parameters,
    /// and removes generic markers from the member name.
    /// </summary>
    /// <param name="value">
    /// The fully qualified member signature, passed by reference. The method may modify this string to remove parameter lists.
    /// </param>
    /// <returns>
    /// A tuple containing the resolved type name and the cleaned member name.
    /// </returns>
    /// <exception cref="InvalidOperationException">
    /// Thrown when the input string does not contain a valid type/member separator.
    /// </exception>
    private static (string typeName, string memberName) ResolveGenericTypeAndMemberName(string value)
    {
        if (value.Contains("("))
        {
            value = value[..value.IndexOf('(')];
        }

        var index = value.LastIndexOf('.');

        if (index == -1)
        {
            throw new InvalidOperationException("Encountered invalid XML node.");
        }

        var (typeName, memberName) = (value[..index], value[(index + 1)..]);

        if (memberName.Contains("("))
        {
            memberName = memberName[..memberName.IndexOf('(')];
        }

        if (memberName.Contains("`"))
        {
            // Remove generic type parameters from the member name
            memberName = memberName[..memberName.IndexOf('`')];
        }

        return (typeName, memberName);
    }

    /// <summary>
    /// Extracts the method parameter type names from a fully qualified member signature.
    /// Handles nested generic parameters and returns a collection of cleaned parameter type names.
    /// </summary>
    /// <param name="value">
    /// The fully qualified member signature, including parameter list (e.g., "Namespace.TypeName.MethodName(System.String, System.Collections.Generic.List&lt;System.Int32&gt;)").
    /// </param>
    /// <returns>
    /// A read-only collection of parameter type names as strings. Returns an empty collection if no parameters are found.
    /// </returns>
    public static IReadOnlyCollection<string> ResolveMethodParameters(string value)
    {
        var startIndex = value.IndexOf('(');

        if (startIndex == -1)
        {
            return [];
        }

        var endIndex = value.LastIndexOf(')');

        if (endIndex <= startIndex)
        {
            return [];
        }

        var parametersString = value.Substring(startIndex + 1, endIndex - startIndex - 1);

        if (string.IsNullOrWhiteSpace(parametersString))
        {
            return [];
        }

        // Handle nested generic parameters by tracking depth
        var parameters = new List<string>();
        var currentParam = "";
        var angleBracketDepth = 0;

        foreach (var c in parametersString)
        {
            if (c == ',' && angleBracketDepth == 0)
            {
                parameters.Add(CleanTypeParameter(currentParam.Trim()));
                currentParam = "";
                continue;
            }

            // Track bracket depth to handle nested generics
            if (c == '<' || c == '{')
            {
                angleBracketDepth++;
            }
            else
            {
                if (c == '>' || c == '}') angleBracketDepth--;
            }

            currentParam += c;
        }

        // Add the last parameter
        if (!string.IsNullOrWhiteSpace(currentParam))
        {
            parameters.Add(CleanTypeParameter(currentParam.Trim()));
        }

        return parameters;
    }

    /// <summary>
    /// Remove generic markers like `0, `1, etc. and any standalone backticks
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    private static string CleanTypeParameter(string value)
    {
        return Regex.Replace(value, @"`\d+", "").Replace("`", "");
    }
}