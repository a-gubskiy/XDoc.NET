using System.Collections.Immutable;
using System.Xml.Linq;

namespace Xdoc;

public record AssemblyXmlInfo
{
    public string Name { get; init; }
    public string Xml { get; init; }

    public IReadOnlyCollection<ClassXmlInfo> Classes { get; init; }

    public AssemblyXmlInfo(string name, string xml)
    {
        Name = name;
        Xml = xml;

        Initialize(xml);
    }

    private void Initialize(string xml)
    {
        var document = XDocument.Parse(xml);

        var groups = document.Descendants("member")
            .GroupBy(m =>
            {
                var attribute = m.Attribute("name");
                var fullName = attribute.Value;

                // Remove the prefix (e.g. "T:", "P:", "M:")
                var name = fullName.Substring(2);

                
                if (fullName.StartsWith("T:"))
                {
                    // If it’s a type (class) member, keep the full string;
                    // otherwise remove the last part (member name) after the dot.
                    
                    return name;
                }
                else
                {
                    var className = name.Substring(0, name.LastIndexOf('.'));
                    return className;
                }
            })
            .ToList();

        foreach (var group in groups)
        {
            
            
        }
    }


    public ClassXmlInfo Get(Type type)
    {
        // Implement

        throw new NotImplementedException();
    }
}