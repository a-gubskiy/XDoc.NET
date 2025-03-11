using TestAssembly.B;

namespace BitzArt.XDoc.Tests;

public class XDocTests
{
    [Fact]
    public void Get_ClassWithThreeFields_ShouldReturnMembersInfo()
    {
        // Arrange
        var xDoc = new XDoc();

        // Act
        var typeDocumentation = xDoc.Get(typeof(Dog));
        var members = typeDocumentation!.MemberData.Keys
            .OrderBy(o => o.Name)
            .ToList();

        // Assert
        Assert.Contains(members, m => m.Name == "Age");
        Assert.Contains(members, m => m.Name == "Name");
        Assert.Contains(members, m => m.Name == "Property1");
        Assert.Contains(members, m => m.Name == "Field2");
        Assert.Contains(members, m => m.Name == "GetInfo");
    }

    [Fact]
    public void Get_PropertyInfo_ShouldReturnPropertyDocumentation()
    {
        // Arrange
        var xDoc = new XDoc();
        var type = typeof(Dog);
        var propertyInfo = type.GetProperty(nameof(Dog.Field3));

        // Act
        var propertyDocumentation = xDoc.Get(propertyInfo!);

        // Assert
        Assert.Equal("Field three", propertyDocumentation!.Node.InnerText.Trim());
    }

    [Fact]
    public void Get_FieldInfo_ShouldReturnFieldDocumentation()
    {
        // Arrange
        var xDoc = new XDoc();
        var type = typeof(Dog);
        var fieldInfo = type.GetField(nameof(Dog.Age));
        var typeDocumentation = xDoc.Get(typeof(Dog));

        // Act
        var fieldDocumentation = typeDocumentation!.GetDocumentation(fieldInfo!);

        // Assert
        Assert.Equal("Dog's Age", fieldDocumentation!.Node.InnerText.Trim());
    }

    [Fact]
    public void Get_FieldInfoForNuGetType_ShouldReturnFieldDocumentation()
    {
        // Arrange
        // var xDoc = new XDoc();
        
        var type = typeof(Newtonsoft.Json.JsonSerializer);

        // var fieldInfo = type.GetProperty(nameof(DateTime.Hour));

        var xmlDocumentationFilePath = XmlUtility.GetXmlDocumentationFilePath(type.Assembly);

        Assert.NotEmpty(xmlDocumentationFilePath);
    }

    [Fact]
    public void Get_MethodInfo_ShouldReturnMethodDocumentation()
    {
        // Arrange
        var xDoc = new XDoc();
        var type = typeof(Dog);
        var methodInfo = type.GetMethod(nameof(Dog.GetInfo));
        var typeDocumentation = xDoc.Get(typeof(Dog));

        // Act
        var methodDocumentation = typeDocumentation!.GetDocumentation(methodInfo!);

        // Assert
        Assert.Equal("Get some about", methodDocumentation!.Node.InnerText.Trim());
    }

    [Fact]
    public void Get_ObjectType_ShouldThrowXDocException()
    {
        // Arrange
        var xDoc = new XDoc();
        var type = typeof(object);
        
        // Act
        var documentation = xDoc.Get(type);

        // Assert
        Assert.Null(documentation);
    }
}