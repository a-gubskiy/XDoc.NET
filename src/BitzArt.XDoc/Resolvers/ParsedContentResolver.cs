// using System.Collections.Frozen;
// using System.Collections.Immutable;
// using System.Reflection;
// using System.Xml;
// using System.Xml.Linq;
//
// namespace BitzArt.XDoc;
//
// /// <summary>
// /// Builds parsed content objects by processing XML documentation nodes
// /// and resolving their references and inheritance hierarchy.
// /// </summary>
// /// <remarks>
// /// This class is responsible for:
// /// - Creating ParsedContent instances from TypeDocumentation and MemberDocumentation
// /// - Resolving XML documentation inheritance chains
// /// - Processing XML documentation references
// /// </remarks>
// internal class ParsedContentResolver
// {
//     private readonly MemberDocumentation _documentation;
//     
//     // private readonly XDoc _source;
//     // private readonly XmlNode? _node;
//
//     private ParsedContentResolver(MemberDocumentation documentation)
//     {
//         _documentation = documentation;
//         // _node = documentation.Node;
//         // _source = documentation.Source;
//     }
//
//     public static ParsedContent Resolve(MemberDocumentation documentation)
//     {
//         return new ParsedContentResolver(documentation).Resolve();
//     }
//
//     private ParsedContent Resolve()
//     {
//         var references = GetReferences();
//
//         return new ParsedContent(_documentation, references);
//     }
//
//     private IReadOnlyDictionary<string, ParsedContent?> GetReferences()
//     {
//         var references = new Dictionary<string, ParsedContent?>();
//
//         var declaredReferences = GetDeclaredReferences(_documentation.Node);
//
//         foreach (var typeName in declaredReferences)
//         {
//             var type = GetTypeInfo(typeName);
//
//             if (type != null)
//             {
//                 var typeDocumentation = _documentation.Source.Get(type);
//
//                 new ParsedContent(documentation, internalReferences);
//
//                 references.Add(typeName, new ParsedContent(GetReferences(typeDocumentation?.Node, _documentation.Source))
//                 {
//                     InheritedContent = GetInheritedContent(typeDocumentation?.Node, _documentation.Source, type)
//                 });
//             }
//             else
//             {
//                 references.Add(typeName, null);
//             }
//         }
//
//         return references;
//     }
//
//     /// <summary>
//     /// Retrieves a collection of declared references from an XML documentation node.
//     /// </summary>
//     /// <param name="xmlNode"></param>
//     /// <returns></returns>
//     private static IReadOnlyCollection<string> GetDeclaredReferences(XmlNode? xmlNode)
//     {
//         if (xmlNode == null || string.IsNullOrWhiteSpace(xmlNode.InnerXml))
//         {
//             return ImmutableList<string>.Empty;
//         }
//
//         var doc = XDocument.Parse(xmlNode.InnerXml);
//
//         var refs = doc.Descendants("see")
//             .Select(e => e.Attribute("cref")?.Value)
//             .Where(value => !string.IsNullOrWhiteSpace(value))
//             .Select(o => o.Substring(2, o.Length - 2))
//             .Distinct()
//             .ToImmutableList();
//
//         return refs;
//     }
//
//     /// <summary>
//     /// 
//     /// </summary>
//     /// <param name="typeName"></param>
//     /// <returns></returns>
//     private static Type? GetTypeInfo(string typeName)
//     {
//         var type = AppDomain.CurrentDomain
//             .GetAssemblies()
//             .Select(a => a.GetType(typeName, false))
//             .FirstOrDefault(t => t != null); // What if we have multiple types with the same name and namespace?
//
//         return type;
//     }
//
//     public static ParsedContent? GetInheritedContent(MemberDocumentation documentation)
//     {
//         var node = documentation.Node;
//             
//         if (node == null)
//         {
//             return null;
//         }    
//         
//         var inheritdocNode = node.ChildNodes.Cast<XmlNode>().FirstOrDefault(o => o.Name == "inheritdoc");
//
//         if (inheritdocNode == null)
//         {
//             return null;
//         }
//
//         var crefAttribute = inheritdocNode.Attributes?["cref"];
//
//         var targetDocumentation= crefAttribute is null ? GetParent(documentation) : GetTarget(crefAttribute);
//
//         var resolver = new ParsedContentResolver(targetDocumentation);
//         
//         return resolver.Resolve();
//     }
//
//     // private ParsedContent? GetInheritedContent()
//     // {
//     //     // if (_node == null)
//     //     // {
//     //     //     return null;
//     //     // }
//     //
//     //     var inheritdocNode = _node.ChildNodes.Cast<XmlNode>().FirstOrDefault(o => o.Name == "inheritdoc");
//     //
//     //     if (inheritdocNode == null)
//     //     {
//     //         return null;
//     //     }
//     //
//     //     var crefAttribute = inheritdocNode.Attributes?["cref"];
//     //
//     //     IDocumentation target = crefAttribute is null ? GetParent() : GetTarget(crefAttribute);
//     //
//     //     return GetInheritedContentByTarget(target);
//     // }
//
//     private static MemberDocumentation GetTarget(XmlAttribute crefAttribute)
//     {
//         var type = crefAttribute.Value.Substring(2, crefAttribute.Value.Length - 2);
//
//         
//     }
//
//     private static MemberDocumentation GetParent(MemberDocumentation documentation)
//     {
//         throw new NotImplementedException();
//     }
//
//     private ParsedContent? GetInheritedContentByTarget(MemberDocumentation documentation)
//     {
//     }
//
//     private ParsedContent? GetParentInheritedContent()
//     {
//         throw new NotImplementedException();
//     }
//
//     /// <summary>
//     /// Retrieves the parent documentation for a member if it uses the inheritdoc tag.
//     /// </summary>
//     /// <param name="xmlNode">The XML documentation node of the member.</param>
//     /// <param name="xDoc">The XDoc instance containing documentation data.</param>
//     /// <param name="memberInfo">The member information to find parent documentation for.</param>
//     /// <returns>A <see cref="ParsedContent"/> object containing the parent's documentation, or null if no inheritance is specified.</returns>
//     private static ParsedContent? GetInheritedContent(XmlNode? xmlNode, XDoc xDoc, MemberInfo memberInfo)
//     {
//         if (xmlNode?.FirstChild?.Name != "inheritdoc")
//         {
//             return null;
//         }
//
//         var parentTypes = GetParentTypes(memberInfo);
//
//         foreach (var parent in parentTypes)
//         {
//             var parentMembers = parent.GetMember(memberInfo.Name);
//
//             foreach (var parentMember in parentMembers)
//             {
//                 if (parentMember is PropertyInfo parentPropertyInfo)
//                 {
//                     var parentPropertyDocumentation = xDoc.Get(parentPropertyInfo);
//
//                     if (parentPropertyDocumentation == null)
//                     {
//                         continue;
//                     }
//
//                     var parentMemberParent = GetInheritedContent(
//                         parentPropertyDocumentation.Node,
//                         xDoc,
//                         parentPropertyDocumentation.DeclaringType);
//
//                     return new ParsedContent(GetReferences(parentPropertyDocumentation?.Node, xDoc))
//                     {
//                         InheritedContent = parentMemberParent
//                     };
//                 }
//                 else if (parentMember is FieldInfo parentFieldInfo)
//                 {
//                     // TODO: implement later
//                     // xDoc.Get(parentFieldInfo);
//                 }
//                 else if (parentMember is MethodInfo parentMethodInfo)
//                 {
//                     // TODO: implement later
//                     // xDoc.Get(parentMethodInfo);
//                 }
//             }
//         }
//
//         return null;
//     }
//
//     /// <summary>
//     /// Retrieves a collection of parent types (interfaces and declaring type) for a given member.
//     /// </summary>
//     /// <param name="memberInfo">The member information to analyze.</param>
//     /// <returns>A frozen set of <see cref="Type"/> objects representing parent types and interfaces.</returns>
//     private static IReadOnlyCollection<Type> GetParentTypes(MemberInfo memberInfo)
//     {
//         var result = new List<Type>();
//
//         var interfaces = memberInfo.DeclaringType?.GetInterfaces() ?? [];
//
//         if (interfaces.Any())
//         {
//             result.AddRange(interfaces);
//         }
//
//         if (memberInfo.DeclaringType != null)
//         {
//             result.Add(memberInfo.DeclaringType);
//         }
//
//         return result.ToFrozenSet();
//     }
// }