namespace BitzArt.XDoc;

/// <summary>
/// Represents documentation for a member of a <see cref="Type"/>.
/// </summary>
public interface IMemberDocumentation
{
    /// <summary>
    /// The <see cref="Type"/> that declares the member.
    /// </summary>
    public Type DeclaringType { get; }
}