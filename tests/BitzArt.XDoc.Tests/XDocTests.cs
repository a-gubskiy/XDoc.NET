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

        Assert.Equal("Age", members[0].Name);
        Assert.Equal("Name", members[1].Name);
        Assert.Equal("Field1", members[2].Name);
        Assert.Equal("Field2", members[3].Name);
    }

    [Fact]
    public void GetDocumentation_PropertyInfo_ShouldReturnPropertyDocumentation()
    {
        var xDoc = new XDoc();

        var type = typeof(Dog);
        var propertyInfo = type.GetProperty(nameof(Dog.Field1));

        var propertyDocumentation = xDoc.GetDocumentation(propertyInfo);

        Assert.Equal("Field one", propertyDocumentation.Node.InnerText.Trim());
    }

    [Fact]
    public void GetDocumentation_FieldInfo_ShouldReturnFieldDocumentation()
    {
        var xDoc = new XDoc();

        var type = typeof(Dog);
        var fieldInfo = type.GetField(nameof(Dog.Age));

        var typeDocumentation = xDoc.GetDocumentation(typeof(Dog));
        var fieldDocumentation = typeDocumentation.GetDocumentation(fieldInfo);

        Assert.Equal("Dog's Age", fieldDocumentation.Node.InnerText.Trim());
    }
    
    [Fact]
    public void GetDocumentation_MethodInfo_ShouldReturnMethodDocumentation()
    {
        var xDoc = new XDoc();

        var type = typeof(Dog);
        var methodInfo = type.GetMethod(nameof(Dog.GetInfo));

        var typeDocumentation = xDoc.GetDocumentation(typeof(Dog));
        var methodDocumentation = typeDocumentation.GetDocumentation(methodInfo);

        Assert.Equal("Get some info", methodDocumentation.Node.InnerText.Trim());
    }

    [Fact]
    public void GetDocumentation__ShouldThrowException()
    {
        var xDoc = new XDoc();

        var type = typeof(object);

        Assert.Throws<XDocException>(() =>
        {
            var documentation = xDoc.GetDocumentation(type);
        });
    }
}