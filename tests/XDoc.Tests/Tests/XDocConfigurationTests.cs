using System.Reflection;

namespace XDoc.Tests;

public class XDocConfigurationTests
{
    private static string Custom_GetXmlDocumentationFilePath_Implementation(Assembly assembly)
    {
        if (assembly == typeof(Console).Assembly)
        {
            return "Resources/System.Console.xml";
        }
        return null!;
    }

    [Fact]
    public void Get_Default_GetXmlDocumentationFilePath_Implementation_ShouldNotReturnDocs()
    {
        var assemblyDocs = XmlUtility.GetXmlDocumentationFilePath(typeof(Console).Assembly);
        Assert.Empty(assemblyDocs);
    }

    [Fact]
    public void Get_Default_GetXmlDocumentationFilePath_Implementation_WithConfiguration_ShouldNotReturnDocs()
    {
        XDocConfiguration configuration = new XDocConfiguration { };
        var assemblyDocs = XmlUtility.GetXmlDocumentationFilePath(typeof(Console).Assembly, configuration);
        Assert.Empty(assemblyDocs);
    }

    [Fact]
    public void Get_Custom_GetXmlDocumentationFilePath_Implementation_ShouldResolveCorrectly()
    {
        XDocConfiguration configuration = new XDocConfiguration
        {
            GetXmlDocumentationFilePath = Custom_GetXmlDocumentationFilePath_Implementation,
        };
        var assemblyDocs = XmlUtility.GetXmlDocumentationFilePath(typeof(Console).Assembly, configuration);
        Assert.NotEmpty(assemblyDocs);
    }
}
