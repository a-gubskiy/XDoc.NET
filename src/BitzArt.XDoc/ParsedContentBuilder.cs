using System.Collections.Frozen;
using System.Collections.Immutable;
using System.Reflection;
using System.Xml;
using System.Xml.Linq;

namespace BitzArt.XDoc;

/// <summary>
/// Builds parsed content objects by processing XML documentation nodes
/// and resolving their references and inheritance hierarchy.
/// </summary>
/// <remarks>
/// This class is responsible for:
/// - Creating ParsedContent instances from TypeDocumentation and MemberDocumentation
/// - Resolving XML documentation inheritance chains
/// - Processing XML documentation references
/// </remarks>
public class ParsedContentBuilder
{
    /// <summary>
    /// Builds a ParsedContent object from <see cref="TypeDocumentation"/>  by processing its XML documentation and resolving references.
    /// </summary>
    /// <param name="typeDocumentation">The type documentation containing XML nodes and type information.</param>
    /// <returns>A ParsedContent object containing resolved documentation, references and inheritance chain.</returns>
    public ParsedContent Build(TypeDocumentation typeDocumentation) =>
        GetParsedContent(typeDocumentation.Node, typeDocumentation.Source, typeDocumentation.Type);

    /// <summary>
    /// Builds a ParsedContent object from <see cref="MemberDocumentation{T}"/> by processing its XML documentation and resolving references.
    /// </summary>
    /// <param name="memberDocumentation">The member documentation containing XML nodes and member information.</param>
    /// <returns>A <see cref="ParsedContent"/> object containing resolved documentation, references and inheritance chain.</returns>
    /// <typeparam name="T">The type of the member being documented.</typeparam>
    public ParsedContent Build<T>(MemberDocumentation<T> memberDocumentation) where T : MemberInfo =>
        GetParsedContent(memberDocumentation.Node, memberDocumentation.Source, memberDocumentation.Member);

    private ParsedContent GetParsedContent<TMember>(XmlNode? xmlNode, XDoc xDoc, TMember memberInfo)
        where TMember : MemberInfo
    {
        var parent = GetParent(xmlNode, xDoc, memberInfo);
        var references = GetReferences(xmlNode, xDoc);

        return new ParsedContent
        {
            Parent = parent,
            References = references,
            Xml = xmlNode,
            Name = memberInfo.Name,
        };
    }

    private ParsedContent GetParsedContent(XmlNode? xmlNode, XDoc xDoc, Type type)
    {
        var parent = GetParent(xmlNode, xDoc, type);
        var references = GetReferences(xmlNode, xDoc);

        return new ParsedContent
        {
            Parent = parent,
            References = references,
            Xml = xmlNode,
            Name = type.Name
        };
    }

    private IReadOnlyDictionary<string, ParsedContent?> GetReferences(XmlNode? node, XDoc xDoc)
    {
        var references = new Dictionary<string, ParsedContent?>();

        var declaredReferences = GetDeclaredReferences(node);

        foreach (var typeName in declaredReferences)
        {
            var type = GetTypeInfo(typeName);

            if (type != null)
            {
                var typeDocumentation = xDoc.Get(type);

                references.Add(typeName, new ParsedContent
                {
                    Name = type.Name,
                    Xml = typeDocumentation?.Node,
                    References = GetReferences(typeDocumentation?.Node, xDoc),
                    Parent = GetParent(typeDocumentation?.Node, xDoc, type)
                });
            }
            else
            {
                references.Add(typeName, null);
            }
        }

        return references;
    }

    /// <summary>
    /// Retrieves a collection of declared references from an XML documentation node.
    /// </summary>
    /// <param name="xmlNode"></param>
    /// <returns></returns>
    internal IReadOnlyCollection<string> GetDeclaredReferences(XmlNode? xmlNode)
    {
        if (xmlNode == null || string.IsNullOrWhiteSpace(xmlNode.InnerXml))
        {
            return ImmutableList<string>.Empty;
        }

        var doc = XDocument.Parse(xmlNode.InnerXml);

        var refs = doc.Descendants("see")
            .Select(e => e.Attribute("cref")?.Value)
            .Where(value => !string.IsNullOrWhiteSpace(value))
            .Select(o => o.Substring(2, o.Length - 2))
            .Distinct()
            .ToImmutableList();

        return refs;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="typeName"></param>
    /// <returns></returns>
    private Type? GetTypeInfo(string typeName)
    {
        var type = AppDomain.CurrentDomain
            .GetAssemblies()
            .Select(a => a.GetType(typeName, false))
            .FirstOrDefault(t => t != null); // What if we have multiple types with the same name and namespace?

        return type;
    }

    private ParsedContent? GetParent(XmlNode? xmlNode, XDoc xDoc, Type type)
    {
        if (xmlNode?.FirstChild?.Name != "inheritdoc" || type.BaseType == null)
        {
            return null;
        }

        var parentTypeDocumentation = xDoc.Get(type.BaseType);

        var parsedContent = GetParsedContent(parentTypeDocumentation?.Node, xDoc, type.BaseType);

        return parsedContent;
    }

    /// <summary>
    /// Retrieves the parent documentation for a member if it uses the inheritdoc tag.
    /// </summary>
    /// <param name="xmlNode">The XML documentation node of the member.</param>
    /// <param name="xDoc">The XDoc instance containing documentation data.</param>
    /// <param name="memberInfo">The member information to find parent documentation for.</param>
    /// <returns>A <see cref="ParsedContent"/> object containing the parent's documentation, or null if no inheritance is specified.</returns>
    private ParsedContent? GetParent(XmlNode? xmlNode, XDoc xDoc, MemberInfo memberInfo)
    {
        if (xmlNode?.FirstChild?.Name == "inheritdoc")
        {
            var parentTypes = GetParentTypes(memberInfo);

            foreach (var parent in parentTypes)
            {
                var parentMembers = parent.GetMember(memberInfo.Name);

                foreach (MemberInfo parentMember in parentMembers)
                {
                    if (parentMember is PropertyInfo parentPropertyInfo)
                    {
                        var parentPropertyDocumentation = xDoc.Get(parentPropertyInfo);

                        if (parentPropertyDocumentation == null)
                        {
                            continue;
                        }

                        var parentMemberParent = GetParent(
                            parentPropertyDocumentation.Node,
                            xDoc,
                            parentPropertyDocumentation.DeclaringType);

                        return new ParsedContent
                        {
                            Name = memberInfo.Name,
                            Xml = parentPropertyDocumentation.Node,
                            References = GetReferences(parentPropertyDocumentation?.Node, xDoc),
                            Parent = parentMemberParent
                        };
                    }
                    else if (parentMember is FieldInfo parentFieldInfo)
                    {
                        // TODO: implement later
                        // xDoc.Get(parentFieldInfo);
                    }
                    else if (parentMember is MethodInfo parentMethodInfo)
                    {
                        // TODO: implement later
                        // xDoc.Get(parentMethodInfo);
                    }
                }
            }
        }

        return null;
    }

    /// <summary>
    /// Retrieves a collection of parent types (interfaces and declaring type) for a given member.
    /// </summary>
    /// <param name="memberInfo">The member information to analyze.</param>
    /// <returns>A frozen set of <see cref="Type"/> objects representing parent types and interfaces.</returns>
    public IReadOnlyCollection<Type> GetParentTypes(MemberInfo memberInfo)
    {
        var result = new List<Type>();

        var interfaces = memberInfo.DeclaringType?.GetInterfaces() ?? [];

        if (interfaces.Any())
        {
            result.AddRange(interfaces);
        }

        if (memberInfo.DeclaringType != null)
        {
            result.Add(memberInfo.DeclaringType);
        }

        return result.ToFrozenSet();
    }
}