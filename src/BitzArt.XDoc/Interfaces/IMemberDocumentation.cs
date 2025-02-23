namespace BitzArt.XDoc;

public interface IMemberDocumentation
{
    /// <summary>
    /// The <see cref="Type"/> that declares the member.
    /// </summary>
    public Type DeclaringType { get; }
}