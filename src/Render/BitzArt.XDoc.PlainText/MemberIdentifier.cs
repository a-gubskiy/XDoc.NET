namespace BitzArt.XDoc.PlainText;

/// <summary>
/// Represents an identifier for types and members in XML documentation comments.
/// </summary>
internal record MemberIdentifier
{
    /// <summary>
    /// Initializes a new instance of the <see cref="MemberIdentifier"/> class by parsing the given string.
    /// </summary>
    /// <param name="value">The cref string to parse (e.g., "T:Namespace.TypeName" or "M:Namespace.TypeName.MethodName").</param>
    /// <exception cref="ArgumentException">Thrown when the cref format is invalid.</exception>
    private MemberIdentifier(string value)
    {
        if (string.IsNullOrEmpty(value))
        {
            throw new ArgumentException("Cref cannot be null or empty", nameof(value));
        }

        if (value.Length < 3 || value[1] != ':')
        {
            throw new ArgumentException($"Invalid cref format: {value}", nameof(value));
        }

        Prefix = value[..2];

        var lastIndexOf = value.LastIndexOf('.');

        switch (Prefix)
        {
            case "T:":
                Type = value.Substring(2, value.Length - 2);
                Member = null;
                break;
            case "M:" or "P:" or "F:":
                Type = value.Substring(2, lastIndexOf - 2);
                Member = value.Substring(lastIndexOf + 1, value.Length - lastIndexOf - 1);
                break;
            default:
                throw new ArgumentException($"Invalid value: {value}");
        }

        var typeLastIndexOf = Type.LastIndexOf('.');

        ShortType = Type.Substring(typeLastIndexOf + 1, Type.Length - typeLastIndexOf - 1);
    }

    /// <summary>
    /// Is the cref a type reference (e.g. "T:")?
    /// </summary>
    public bool IsType => Prefix is "T:";

    /// <summary>
    /// Is the cref a member reference (e.g. "M:", "P:", "F:")?
    /// </summary>
    public bool IsMember => Prefix is "M:" or "P:" or "F:";

    /// <summary>
    /// The prefix of the cref (e.g. "T:", "M:", "P:", "F:").
    /// </summary>
    public string Prefix { get; init; }

    /// <summary>
    /// The type name
    /// </summary>
    public string Type { get; init; }

    /// <summary>
    /// The short type name (without namespace)
    /// </summary>
    public string ShortType { get; init; }

    /// <summary>
    /// Method, property, or field name if present in the cref.
    /// Will be null for type references.
    /// </summary>
    public string? Member { get; init; }

    /// <summary>
    /// Returns a string that represents the current object.
    /// Formats the identifier back into a valid cref string.
    /// </summary>
    /// <returns>A string that represents the current object.</returns>
    public override string ToString() => $"{Prefix}{Type}{(Member is not null ? "." + Member : string.Empty)}";

    /// <summary>
    /// Attempts to create a <see cref="MemberIdentifier"/> from a string value.
    /// </summary>
    /// <param name="value">The cref string to parse.</param>
    /// <param name="cref">When this method returns, contains the created <see cref="MemberIdentifier"/> if successful; otherwise, null.</param>
    /// <returns>true if the creation was successful; otherwise, false.</returns>
    public static bool TryCreate(string value, out MemberIdentifier? cref)
    {
        try
        {
            cref = new MemberIdentifier(value);
        }
        catch
        {
            cref = null;
        }

        return cref != null;
    }
}