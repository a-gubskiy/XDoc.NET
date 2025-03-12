using System.Xml;
using TestAssembly.B;

namespace BitzArt.XDoc.Tests;

public class ParsedContentResolverTests
{
    
    [Fact]
    public void Build_TypeWithoutReferences_ShouldReturnBasicParsedContent()
    {
        
        // Arrange
        var xDoc = new XDoc();
        var type = typeof(Dog);
        var propertyInfo = type.GetProperty(nameof(Dog.Property1));
        
        var propertyDocumentation = xDoc.Get(type)?.GetDocumentation(propertyInfo!);

        var inheritanceMemberDocumentationReference = propertyDocumentation?.Inherited;

        Assert.NotNull(propertyDocumentation);
        Assert.NotNull(inheritanceMemberDocumentationReference);
    }
}