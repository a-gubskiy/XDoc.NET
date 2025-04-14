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
        MemberDocumentation<PropertyInfo> memberDocumentation = new PropertyDocumentation(
            source: new XDoc(),
            property: typeof(DemoClass).GetProperty(nameof(DemoClass.MyProperty))!,
            node: null);

        var inheritanceTarget = memberDocumentation.GetInheritanceTarget();

        Assert.NotNull(inheritanceTarget);
    }

    [Fact]
    public void GetInheritanceTarget_PropertyHasNoParent_ShouldReturnNull()
    {
        MemberDocumentation<PropertyInfo> memberDocumentation = new PropertyDocumentation(
            source: new XDoc(),
            property: typeof(DemoClass).GetProperty(nameof(DemoClass.MyOtherProperty))!,
            node: null);

        var inheritanceTarget = memberDocumentation.GetInheritanceTarget();

        Assert.Null(inheritanceTarget);
    }
}