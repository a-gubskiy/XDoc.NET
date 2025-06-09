namespace BitzArt.XDoc;

public class QualifiedMemberNameParser
{
    /// <summary>
    /// Resolves a qualified member name into its associated type and member name.
    /// Handles special cases like generic types and methods with parameters.
    /// </summary>
    /// <param name="name">The fully qualified member name (e.g. "Namespace.TypeName.MemberName")</param>
    /// <returns>A tuple containing the resolved Type and the simple member name</returns>
    /// <exception cref="InvalidOperationException">
    /// Thrown when the name doesn't contain a type/member separator or when the type cannot be found
    /// </exception>
    public static (string type, string memberName) ResolveTypeAndMemberName(string name)
    {
        if (name.Contains('`'))
        {
            return ResolveGenericTypeAndMemberName(ref name);
        }

        if (name.Contains('('))
        {
            name = name[..name.IndexOf('(')];
        }

        return SplitTypeAndMemberName(name);
    }

    private static (string type, string memberName) ResolveGenericTypeAndMemberName(ref string name)
    {
        if (name.Contains("("))
        {
            name = name[..name.IndexOf('(')];
        }
        
        var index = name.LastIndexOf('.');

        if (index == -1)
        {
            throw new InvalidOperationException("Encountered invalid XML node.");
        }

        var (typeName, memberName) = (name[..index], name[(index + 1)..]);
        
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
    /// Splits a fully qualified member name into type name and member name.
    /// </summary>
    /// <param name="name">The fully qualified name in the format "Namespace.TypeName.MemberName"</param>
    /// <returns>A tuple containing the resolved Type object and the simple member name</returns>
    /// <exception cref="InvalidOperationException">
    /// Thrown when the name doesn't contain a type/member separator (period) 
    /// or when the type cannot be found in the assembly
    /// </exception>
    public static (string type, string memberName) SplitTypeAndMemberName(string name)
    {
        var index = name.LastIndexOf('.');

        if (index == -1)
        {
            throw new InvalidOperationException("Encountered invalid XML node.");
        }

        var (typeName, memberName) = (name[..index], name[(index + 1)..]);

        // var type = _assembly.GetType(typeName)
        //            ?? throw new InvalidOperationException($"Type '{typeName}' not found.");

        return (typeName, memberName);
    }
    
    public static IReadOnlyCollection<string> ResolveMethodParameters(string name)
    {
        var startIndex = name.IndexOf('(');
    
        if (startIndex == -1)
        {
            return Array.Empty<string>();
        }
    
        var endIndex = name.LastIndexOf(')');
    
        if (endIndex <= startIndex)
        {
            return Array.Empty<string>();
        }
    
        var parametersString = name.Substring(startIndex + 1, endIndex - startIndex - 1);
    
        if (string.IsNullOrWhiteSpace(parametersString))
        {
            return Array.Empty<string>();
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
            if (c == '<' || c == '{') angleBracketDepth++;
            else if (c == '>' || c == '}') angleBracketDepth--;
            
            currentParam += c;
        }
        
        // Add the last parameter
        if (!string.IsNullOrWhiteSpace(currentParam))
        {
            parameters.Add(CleanTypeParameter(currentParam.Trim()));
        }
    
        return parameters;
    }
    
    private static string CleanTypeParameter(string parameter)
    {
        // Remove generic markers like `0, `1, etc.
        var result = parameter;
        
        // Use regex to replace all occurrences of `N
        result = System.Text.RegularExpressions.Regex.Replace(result, @"`\d+", "");
        
        // Remove any standalone backticks
        result = result.Replace("`", "");
        
        return result;
    }
}