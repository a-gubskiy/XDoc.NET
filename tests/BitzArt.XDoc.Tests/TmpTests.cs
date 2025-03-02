using System.Reflection;
using System.Reflection.Metadata;
using System.Reflection.PortableExecutable;
using System.Xml.Linq;

namespace BitzArt.XDoc.Tests;

public class TmpTests
{
    [Fact]
    public void Get_TypedInfo_ShouldReturnFieldDocumentation()
    {
        var description = GetDocumentation(typeof(int));
        
        Assert.NotEmpty(description);
    }
    
    
    // [Fact]
    // public void Get_PropertiInfo_ShouldReturnFieldDocumentation()
    // {
    //     var dateTime = DateTime.Now;
    //     var type = dateTime.GetType();
    //     var propertyInfo = type.GetProperty(nameof(DateTime.Hour));
    //
    //     var description = GetDocumentation(propertyInfo);
    //     
    //     Assert.NotEmpty(description);
    // }
    
    
   
    
    public static string GetDocumentation(Type type)
    {
        var assemblyPath = type.Assembly.Location;
        using var peReader = new PEReader(File.OpenRead(assemblyPath));
        var metadataReader = peReader.GetMetadataReader();
        
        // Implement documentation extraction using the metadata reader
        // This requires more code to navigate the metadata structure

        return "";
    }
    
}


