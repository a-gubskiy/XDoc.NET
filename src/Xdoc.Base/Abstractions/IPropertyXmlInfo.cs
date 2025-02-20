namespace Xdoc.Abstractions;


public interface IPropertyXmlInfo : ISummarized
{
    /// <summary>
    /// Name of the property.
    /// </summary>
    string Name { get; }

    /// <summary>
    /// Class which the property belongs to.
    /// </summary>
    IClassXmlInfo Class { get; }
}