using System.Reflection;
using System.Xml;

namespace BitzArt.XDoc.PlainText.Tests;

abstract class BaseClass
{
    /// <summary>
    /// My comment
    /// </summary>
    public abstract int MyProperty { get; set; }
}

class DemoClass : BaseClass
{
    public override int MyProperty { get; set; }

    public int MyOtherProperty { get; set; }
}

public class MemberDocumentationTests
{
    [Fact]
    public void GetInheritanceTarget_PropertyHasParent_ShouldReturnParentProperty()
    {
        var property = typeof(DemoClass).GetProperty(nameof(DemoClass.MyProperty));
        var xmlDocument = new XmlDocument();
        var textNode = xmlDocument.CreateTextNode("This is a test property.");

        MemberDocumentation<PropertyInfo> memberDocumentation = new PropertyDocumentation(
            source: new XDoc(),
            property: property!,
            node: textNode);

        var inheritanceTarget = memberDocumentation.GetInheritanceTarget();

        Assert.NotNull(inheritanceTarget);
    }

    [Fact]
    public void GetInheritanceTarget_PropertyHasNoParent_ShouldReturnNull()
    {
        var property = typeof(DemoClass).GetProperty(nameof(DemoClass.MyOtherProperty));
        var xmlDocument = new XmlDocument();
        var textNode = xmlDocument.CreateTextNode("This is a test property.");

        MemberDocumentation<PropertyInfo> memberDocumentation = new PropertyDocumentation(
            source: new XDoc(),
            property: property!,
            node: textNode);

        var inheritanceTarget = memberDocumentation.GetInheritanceTarget();

        Assert.Null(inheritanceTarget);
    }
}