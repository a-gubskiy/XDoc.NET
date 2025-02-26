using TestAssembly.B;
using Xdoc.Renderer.PlaintText;

namespace BitzArt.XDoc.PlaintText.Tests;

public class XDocExtensionsTests
{
    [Fact]
    public void ToPlainText_DogClass_ShouldReturnDescription()
    {
        var xDoc = new XDoc();
        var type = typeof(Dog);

        var str = xDoc.Get(type).ToPlainText();

        Assert.NotEmpty(str);
    }

    [Fact]
    public void ToPlainText_DogNameProperty_ShouldReturnDescription()
    {
        var xDoc = new XDoc();

        var str = xDoc.Get(typeof(Dog).GetProperty(nameof(Dog.Name))!).ToPlainText();

        Assert.NotEmpty(str);
    }

    [Fact]
    public void ToPlainText_DogProperties_ShouldReturnDescriptions()
    {
        var xDoc = new XDoc();
        var type = typeof(Dog);

        var field1Info = type.GetProperty(nameof(Dog.Field1));
        var field2Info = type.GetProperty(nameof(Dog.Field2));
        var nameInfo = type.GetProperty(nameof(Dog.Name));
        var idInfo = type.GetProperty(nameof(Dog.Id));

        var filed1Comment = xDoc.Get(field1Info!).ToPlainText();
        var filed2Comment = xDoc.Get(field2Info!).ToPlainText();
        var nameComment = xDoc.Get(nameInfo!).ToPlainText();
        var propertyDocumentation = xDoc.Get(idInfo!);
        var idComment = propertyDocumentation.ToPlainText();

        Assert.NotEmpty(filed1Comment);
        Assert.NotEmpty(filed2Comment);
        Assert.NotEmpty(nameComment);
        Assert.NotEmpty(idComment);
    }
}