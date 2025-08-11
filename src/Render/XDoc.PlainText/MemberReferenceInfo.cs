namespace XDoc.PlainText;

/// <summary>
/// Represents an identifier for types and members in XML documentation comments.
/// </summary>
internal record MemberReferenceInfo
{
    private MemberReferenceInfo()
    {
    }

    /// <summary>
    /// Determines if the identifier is a type reference (e.g. "T:").
    /// </summary>
    public required bool IsType { get; set; }

    /// <summary>
    /// Determines if the identifier is a member reference (e.g. "M:", "P:", "F:").
    /// </summary>
    public required bool IsMember { get; set; }

    /// <summary>
    /// The type name
    /// </summary>
    public required string Type { get; init; }

    /// <summary>
    /// The short type name (without namespace)
    /// </summary>
    public required string ShortType { get; init; }

    /// <summary>
    /// The member name (e.g. method, property, field).
    /// </summary>
    public required string? Member { get; init; }

    /// <summary>
    /// The original string value of the identifier.
    /// </summary>
    public required string Value { get; init; }

    /// <summary>
    /// List if supported member prefixes.
    /// </summary>
    private static readonly IReadOnlyCollection<string> AllowedPrefixes = ["T:", "M:", "P:", "F:"];

    /// <summary>
    /// Returns a string that represents the current object.
    /// Formats the identifier back into a valid member or type identifier string.
    /// </summary>
    /// <returns>A string that represents the current object.</returns>
    public override string ToString() => Value;

    /// <summary>
    /// Validates the provided reference string and attempts to create a <see cref="MemberReferenceInfo"/> based on it.
    /// </summary>
    /// <param name="value">The string to parse.</param>
    /// <returns>The created <see cref="MemberReferenceInfo"/> if successful; otherwise, null.</returns>
    public static MemberReferenceInfo? FromReferenceString(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return null;
        }

        if (value.Length < 3 || value[1] != ':')
        {
            return null;
        }

        var prefix = value[..2];

        if (!AllowedPrefixes.Contains(prefix))
        {
            return null;
        }

        var lastIndexOfDot = value.LastIndexOf('.');

        var type = prefix switch
        {
            "T:" => value.Substring(2, value.Length - 2),
            "M:" or "P:" or "F:" => value.Substring(2, lastIndexOfDot - 2),
            _ => string.Empty
        };

        var member = prefix switch
        {
            "T:" => null,
            "M:" or "P:" or "F:" => value.Substring(lastIndexOfDot + 1, value.Length - lastIndexOfDot - 1),
            _ => string.Empty
        };

        var typeLastIndexOfDot = type.LastIndexOf('.');
        var shortType = type.Substring(typeLastIndexOfDot + 1, type.Length - typeLastIndexOfDot - 1);

        var memberPrefixes = new[] { "M:", "P:", "F:" };

        return new MemberReferenceInfo
        {
            IsType = prefix == "T:",
            IsMember = memberPrefixes.Contains(prefix),
            Type = type,
            ShortType = shortType,
            Member = member,
            Value = value
        };
    }
}