// using System.Xml;
//
// namespace BitzArt.XDoc.Tests;
//
// public class DocumentationReferenceResolverTests
// {
//     private readonly DocumentationReferenceResolver _resolver;
//     private readonly XDoc _source;
//
//     public DocumentationReferenceResolverTests()
//     {
//         _resolver = new DocumentationReferenceResolver();
//         _source = new XDoc();
//     }
//
//     [Fact]
//     public void ElementWithValidCrefAttribute_ReturnsReference()
//     {
//         var doc = new XmlDocument();
//         var node = doc.CreateElement("param");
//         var crefAttr = doc.CreateAttribute("cref");
//         crefAttr.Value = "T:System.String";
//         node.Attributes.Append(crefAttr);
//
//         var result = _resolver.GetReference(_source, node);
//
//         Assert.NotNull(result);
//         Assert.Equal("T:System.String", result.Cref.ToString());
//         Assert.Same(node, result.RequirementNode);
//     }
//
//     [Fact]
//     public void InheritdocElement_ReturnsReference()
//     {
//         var doc = new XmlDocument();
//         var node = doc.CreateElement("inheritdoc");
//
//         var result = _resolver.GetReference(_source, node);
//
//         Assert.NotNull(result);
//         Assert.Null(result.Cref);
//         Assert.Same(node, result.RequirementNode);
//     }
//
//     [Fact]
//     public void SeeElement_ReturnsReference()
//     {
//         var doc = new XmlDocument();
//         var node = doc.CreateElement("see");
//
//         var result = _resolver.GetReference(_source, node);
//
//         Assert.NotNull(result);
//         Assert.Null(result.Cref);
//         Assert.Same(node, result.RequirementNode);
//     }
//
//     [Fact]
//     public void SeeElementWithCref_ReturnsReferenceWithCref()
//     {
//         var doc = new XmlDocument();
//         var node = doc.CreateElement("see");
//         var crefAttr = doc.CreateAttribute("cref");
//         crefAttr.Value = "T:System.Int32";
//         node.Attributes.Append(crefAttr);
//
//         var result = _resolver.GetReference(_source, node);
//
//         Assert.NotNull(result);
//         Assert.Equal("T:System.Int32", result.Cref.ToString());
//         Assert.Same(node, result.RequirementNode);
//     }
//
//     [Fact]
//     public void RegularElementWithNoCref_ReturnsNull()
//     {
//         var doc = new XmlDocument();
//         var node = doc.CreateElement("summary");
//
//         var result = _resolver.GetReference(_source, node);
//
//         Assert.Null(result);
//     }
//
//     [Fact]
//     public void RegularElementWithEmptyCref_ReturnsNull()
//     {
//         var doc = new XmlDocument();
//         var node = doc.CreateElement("param");
//         var crefAttr = doc.CreateAttribute("cref");
//         crefAttr.Value = "";
//         node.Attributes.Append(crefAttr);
//
//         var result = _resolver.GetReference(_source, node);
//
//         Assert.Null(result);
//     }
//
//     [Fact]
//     public void RegularElementWithWhitespaceCref_ReturnsNull()
//     {
//         var doc = new XmlDocument();
//         var node = doc.CreateElement("param");
//         var crefAttr = doc.CreateAttribute("cref");
//         crefAttr.Value = "   ";
//         node.Attributes.Append(crefAttr);
//
//         var result = _resolver.GetReference(_source, node);
//
//         Assert.Null(result);
//     }
//
//     [Fact]
//     public void ElementWithNullAttributes_ReturnsNull()
//     {
//         var doc = new XmlDocument();
//         var node = doc.CreateElement("param");
//         // No attributes added
//
//         var result = _resolver.GetReference(_source, node);
//
//         Assert.Null(result);
//     }
// }