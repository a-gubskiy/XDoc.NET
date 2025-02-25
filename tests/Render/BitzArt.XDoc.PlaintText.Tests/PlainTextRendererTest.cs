using System.Reflection;
using TestAssembly.B;
using Xdoc.Renderer.PlaintText;

namespace BitzArt.XDoc.PlaintText.Tests;

public class UnitTest1
{
    [Fact]
    public async Task ToPlainText_Type()
    {
        var xDoc = new XDoc();
        var type = typeof(Dog);
        var str = xDoc.Get(type).ToPlainText();

        Assert.NotEmpty(str);
    }

    [Fact]
    public async Task ToPlainText_PropertyInfo()
    {
        var xDoc = new XDoc();
        var type = typeof(Dog);

        var propertyInfo = type.GetProperty(nameof(Dog.Field1));

        var str = xDoc.Get(propertyInfo).ToPlainText();
        Assert.NotEmpty(str);
    }
}