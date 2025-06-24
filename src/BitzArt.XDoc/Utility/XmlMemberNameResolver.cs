using System.Text.RegularExpressions;

namespace BitzArt.XDoc;

/// <summary>
/// Provides utility methods for parsing and resolving information from fully qualified member signatures,
/// such as extracting type names, member names, and method parameter types. 
/// Handles special cases including generic types and nested generic parameters.
/// </summary>
internal static partial class XmlMemberNameResolver
{
    public record TypeAndMemberName(string TypeName, string MemberName);

    /// <summary>
    /// Resolves a qualified member name into its associated type and member name.
    /// Handles special cases like generic types and methods with parameters.
    /// </summary>
    /// <param name="xmlDocumentationMemberName">
    /// The fully qualified member name with prefix (e.g. "P:Company.Name.Space.TypeName.MemberName")
    /// </param>
    /// <returns>A tuple containing the resolved Type and the simple member name</returns>
    /// <exception cref="InvalidOperationException">
    /// Thrown when the name doesn't contain a type/member separator or when the type cannot be found
    /// </exception>
    public static TypeAndMemberName ResolveTypeAndMemberName(string xmlDocumentationMemberName)
    {
        // A member name containing an opening parenthesis indicates a method.
        if (xmlDocumentationMemberName.Contains('('))
        {
            // Remove method parameter information from the XML documentation member name by 
            // truncating the string at the opening parenthesis, keeping only the method name part.
            // This ensures we extract just the method name without its parameter signature.
            xmlDocumentationMemberName = xmlDocumentationMemberName[..xmlDocumentationMemberName.IndexOf('(')];
        }

        if (!xmlDocumentationMemberName.Contains('.'))
        {
            throw new InvalidOperationException(
                $"XML documentation member name '{xmlDocumentationMemberName}' does not contain a type separator.");
        }

        // Find the position of the last dot in the member name, which separates
        // the type name from the member name (e.g., "Namespace.TypeName.MemberName" -> position of last dot)
        var indexOfLastDot = xmlDocumentationMemberName.LastIndexOf('.');

        var typeName = xmlDocumentationMemberName[..indexOfLastDot];
        var memberName = xmlDocumentationMemberName[(indexOfLastDot + 1)..];

        // Backtick (`) => generic type or method.
        if (xmlDocumentationMemberName.Contains('`'))
        {
            // Opening parenthesis indicates a method with parameters.
            if (memberName.Contains('('))
            {
                memberName = memberName[..memberName.IndexOf('(')];
            }

            // A backtick (`) in the member name
            // indicates generic type parameters.
            if (memberName.Contains('`'))
            {
                // Remove generic type parameters from the member name
                memberName = memberName[..memberName.IndexOf('`')];
            }
        }

        return new(typeName, memberName);
    }

    /// <summary>
    /// Extracts the method parameter type names from a fully qualified member signature.
    /// Handles nested generic parameters and returns a collection of cleaned parameter type names.
    /// </summary>
    /// <param name="xmlDocumentationMemberName">
    /// The fully qualified member signature, including parameter list (e.g., "Namespace.TypeName.MethodName(System.String, System.Collections.Generic.List&lt;System.Int32&gt;)").
    /// </param>
    /// <returns>
    /// A read-only collection of parameter type names as strings. Returns an empty collection if no parameters are found.
    /// </returns>
    public static IReadOnlyCollection<string> ResolveMethodParameters(string xmlDocumentationMemberName)
    {
        var parameterListStartIndex = xmlDocumentationMemberName.IndexOf('(');

        if (parameterListStartIndex == -1)
        {
            // No parameter list found
            return [];
        }

        var parameterListEndIndex = xmlDocumentationMemberName.LastIndexOf(')');

        if (parameterListEndIndex <= parameterListStartIndex)
        {
            throw new InvalidOperationException(
                $"XML documentation member '{xmlDocumentationMemberName}' parameter list is invalid.");
        }

        var parametersString = xmlDocumentationMemberName.Substring(
            parameterListStartIndex + 1,
            parameterListEndIndex - parameterListStartIndex - 1);

        if (string.IsNullOrWhiteSpace(parametersString))
        {
            // No parameters found
            return [];
        }

        // Handle nested generic parameters while tracking nesting depth
        return GetParameters(parametersString);
    }
    
    /// <summary>
    /// Recursively splits a comma-delimited parameter string at top-level commas.
    /// </summary>
    private static List<string> GetParameters(string value)
    {
        // Base case: nothing left?
        if (string.IsNullOrWhiteSpace(value))
        {
            return [];
        }

        // Find the position of the first top-level comma
        var commaPosition = FindCommaPosition(value, 0, 0);
        
        if (commaPosition < 0)
        {
            // This is a single parameter
            return [value.Trim()];
        }

        // Split off the first parameter, then recurse on the rest
        var first = value[..commaPosition].Trim();
        
        var result = new List<string> { first };

        var parameters = GetParameters(value[(commaPosition + 1)..]);
        result.AddRange(parameters);
        
        return result;
    }

    /// <summary>
    /// Recursively scans for a comma at depth==0 (i.e. not inside any {} pairs).
    /// Returns its index, or -1 if none.
    /// </summary>
    private static int FindCommaPosition(string value, int index, int depth)
    {
        if (index >= value.Length)
        {
            // End of string reached without finding a top-level comma
            return -1;
        }

        var c = value[index];

        var topLevelCommaPosition = c switch
        {
            '{' or '<' => FindCommaPosition(value, index + 1, depth + 1),
            '}' or '>' => FindCommaPosition(value, index + 1, depth - 1),
            ',' when depth == 0 => index,
            _ => FindCommaPosition(value, index + 1, depth)
        };
        
        return topLevelCommaPosition;
    }

    /// <summary>
    /// Matches generic markers like `0, `1, etc. and any standalone backticks
    /// </summary>
    /// <returns></returns>
    [GeneratedRegex(@"\`\d+|\`")]
    private static partial Regex GetGenericMarkerRegex();

    internal static string RemoveGenericMarkers(string value) => GetGenericMarkerRegex().Replace(value, string.Empty);
}