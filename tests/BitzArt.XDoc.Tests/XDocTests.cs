using TestAssembly.B;

namespace BitzArt.XDoc.Tests;

public class XDocTests
{
    [Fact]
    public void GetDocumentation_ClassWithThreeFields_ShouldReturnMembersInfo()
    {
        var xDoc = new XDoc();

        var typeDocumentation = xDoc.GetDocumentation(typeof(Dog));

        var members = typeDocumentation.MemberData.Keys.ToList();

        Assert.Equal("Name", members[0].Name);
        Assert.Equal("Field1", members[1].Name);
        Assert.Equal("Field2", members[2].Name);
    }
    
    [Fact]
    public void GetDocumentation_PropertyInfo_ShouldReturnPropertyDocumentation()
    {
        var xDoc = new XDoc();

        var type = typeof(Dog);
        var propertyInfo = type.GetProperty("Field1");

        var propertyDocumentation = xDoc.GetDocumentation(propertyInfo);

        Assert.Equal("Field one", propertyDocumentation.Node.InnerText.Trim());
    } 
    
    [Fact]
    public void GetDocumentation_FieldInfo_ShouldReturnFieldDocumentation()
    {
        var xDoc = new XDoc();

        var type = typeof(Dog);
        var fieldInfo = type.GetField("Age");
        
        var typeDocumentation = xDoc.GetDocumentation(typeof(Dog));
        var fieldDocumentation = typeDocumentation.GetDocumentation(fieldInfo);

        Assert.Equal("Dog's Age", fieldDocumentation.Node.InnerText.Trim());
    }
}