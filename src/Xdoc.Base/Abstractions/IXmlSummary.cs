using System.Xml;

namespace Xdoc.Abstractions;

public interface IXmlSummary
{
    XmlNode? Xml { get; }
}