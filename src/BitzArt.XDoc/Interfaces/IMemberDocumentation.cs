namespace BitzArt.XDoc;

internal interface IMemberDocumentation
{
    /// <summary>
    /// The <see cref="Type"/> that declares the member.
    /// </summary>
    public Type DeclaringType { get; }
}