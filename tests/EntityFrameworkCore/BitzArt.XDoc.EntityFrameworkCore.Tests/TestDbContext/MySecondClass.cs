namespace BitzArt.XDoc.Tests;

public class MySecondClass
{
    public const string NullableNameComment = "Nullable Name Comment";

    public const string NullableValueComment = "Nullable Value Comment";

    public const string NullableDoubleComment = "Nullable Double Comment";

    public const string NameComment = "Name Comment";

    /// <summary>
    /// Name Comment
    /// </summary>
    public string Name { get; set; } = null!;

    /// <summary>
    /// Nullable Name Comment
    /// </summary>
    public string? NullableName { get; set; }

    /// <summary>
    /// Nullable Value Comment
    /// </summary>
    public int? NullableValue { get; set; }

    /// <summary>
    /// Nullable Double Comment
    /// </summary>
    public double? NullableDouble;
}
