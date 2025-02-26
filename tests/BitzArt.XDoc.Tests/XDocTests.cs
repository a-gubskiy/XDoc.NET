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
            .OrderBy(o => o)
            .ToList();

        // Assert
        Assert.Contains(members, m => m == "Age");
        Assert.Contains(members, m => m == "Name");
        Assert.Contains(members, m => m == "Field1");
        Assert.Contains(members, m => m == "Field2");
        Assert.Contains(members, m => m == "GetInfo");
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

        // Act + Assert
        Assert.Throws<XDocException>(() =>
        {
            var documentation = xDoc.Get(type);
        });
    }
}