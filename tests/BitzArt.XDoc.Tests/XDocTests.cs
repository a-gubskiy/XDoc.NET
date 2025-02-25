using TestAssembly.B;

namespace BitzArt.XDoc.Tests;

public class XDocTests
{
    [Fact]
    public void GetDocumentation_ClassWithThreeFields_ShouldReturnMembersInfo()
    {
        var xDoc = new XDoc();

        var typeDocumentation = xDoc.Get(typeof(Dog));

        var members = typeDocumentation!.MemberData.Keys
            .OrderBy(o => o.Name)
            .ToList();

        Assert.Contains(members, m => m.Name == "Age");
        Assert.Contains(members, m => m.Name == "Name");
        Assert.Contains(members, m => m.Name == "Field1");
        Assert.Contains(members, m => m.Name == "Field2");
        Assert.Contains(members, m => m.Name == "GetInfo");
    }

    [Fact]
    public void GetDocumentation_PropertyInfo_ShouldReturnPropertyDocumentation()
    {
        var xDoc = new XDoc();

        var type = typeof(Dog);
        var propertyInfo = type.GetProperty(nameof(Dog.Field3));

        var propertyDocumentation = xDoc.Get(propertyInfo!);

        Assert.Equal("Field one", propertyDocumentation!.Node.InnerText.Trim());
    }

    [Fact]
    public void GetDocumentation_FieldInfo_ShouldReturnFieldDocumentation()
    {
        var xDoc = new XDoc();

        var type = typeof(Dog);
        var fieldInfo = type.GetField(nameof(Dog.Age));

        var typeDocumentation = xDoc.Get(typeof(Dog));
        var fieldDocumentation = typeDocumentation!.GetDocumentation(fieldInfo!);

        Assert.Equal("Dog's Age", fieldDocumentation!.Node.InnerText.Trim());
    }

    [Fact]
    public void GetDocumentation_MethodInfo_ShouldReturnMethodDocumentation()
    {
        var xDoc = new XDoc();

        var type = typeof(Dog);
        var methodInfo = type.GetMethod(nameof(Dog.GetInfo));

        var typeDocumentation = xDoc.Get(typeof(Dog));
        var methodDocumentation = typeDocumentation!.GetDocumentation(methodInfo!);

        Assert.Equal("Get some info", methodDocumentation!.Node.InnerText.Trim());
    }

    [Fact]
    public void GetDocumentation__ShouldThrowException()
    {
        var xDoc = new XDoc();

        var type = typeof(object);

        Assert.Throws<XDocException>(() =>
        {
            var documentation = xDoc.Get(type);
        });
    }
}