using System.Xml;

namespace BitzArt.XDoc.Tests;

/// <summary>
/// Comment on MyBaseClass
/// </summary>
abstract class MyBaseClass
{
    /// <summary>
    /// My method in MyBaseClass
    /// </summary>
    public abstract string MyMethod();
}

class MyClassA : MyBaseClass
{
    public override string MyMethod() => "MyClassA.MyMethod";
}

/// <summary>
/// Comment on IMyInterface
/// </summary>
interface IMyInterface
{
    /// <summary>
    /// My method in IMyInterface
    /// </summary>
    string MyMethod();
}

class MyClassB : IMyInterface
{
    public string MyMethod() => "MyClassB.MyMethod";
}

/// <inheritdoc />
interface IMyInterface1 : IMyInterface
{
    int MethodA();
}

interface IMyInterface2
{
    int MethodB();
}

/// <inheritdoc />
class MyClassC : IMyInterface1, IMyInterface2
{
    public string MyMethod() => "MyClassC.MyMethod";

    public int MethodA() => 0;

    public int MethodB() => 0;
}

class MyClassD : MyClassA
{
}

class MyClassWithMultipleInheritance : MyBaseClass, IMyInterface
{
    public override string MyMethod() => "MyClassWithMultipleInheritance.MyMethod";
}

public class InheritanceResolverTests
{
    private const string MyMethodInIMyInterface = "My method in IMyInterface";
    private const string MyMethodInMyBaseClass = "My method in MyBaseClass";
    private const string CommentOnMyBaseClass = "Comment on MyBaseClass";

    [Fact]
    public void GetTargetMember_CommentOnMethodInBaseClass_ShouldReturnComment()
    {
        XmlNode? node = null;
        var xdoc = new XDoc();
        var methodInfo = typeof(MyClassA).GetMethod(nameof(MyClassA.MyMethod))!;

        var targetMember = InheritanceResolver.GetTargetMember(methodInfo, node);
        var documentationElement = xdoc.Get(targetMember!);

        Assert.NotNull(documentationElement);
        Assert.Equal(MyMethodInMyBaseClass, documentationElement.Text);
    }

    [Fact]
    public void GetTargetMember_CommentOnMethodInBaseClassOfBaseClass_ShouldReturnComment()
    {
        XmlNode? node = null;
        var xdoc = new XDoc();
        var methodInfo = typeof(MyClassD).GetMethod(nameof(MyClassD.MyMethod))!;

        var targetMember = InheritanceResolver.GetTargetMember(methodInfo, node);
        var documentationElement = xdoc.Get(targetMember!);

        Assert.NotNull(documentationElement);
        Assert.Equal(MyMethodInMyBaseClass, documentationElement.Text);
    }

    [Fact]
    public void GetTargetMember_CommentOnMethodInBaseInterface_ShouldReturnComment()
    {
        XmlNode? node = null;
        var xdoc = new XDoc();
        var methodInfo = typeof(MyClassB).GetMethod(nameof(MyClassB.MyMethod))!;

        var targetMember = InheritanceResolver.GetTargetMember(methodInfo, node);
        var documentationElement = xdoc.Get(targetMember!);

        Assert.NotNull(documentationElement);
        Assert.Equal(MyMethodInIMyInterface, documentationElement.Text);
    }

    [Fact]
    public void GetTargetMember_CommentOnMethodInBaseInterfaceOfBaceInterace_ShouldReturnComment()
    {
        XmlNode? node = null;
        var xdoc = new XDoc();
        var methodInfo = typeof(MyClassC).GetMethod(nameof(MyClassC.MyMethod))!;

        var targetMember = InheritanceResolver.GetTargetMember(methodInfo, node);
        var documentationElement = xdoc.Get(targetMember!);

        Assert.NotNull(documentationElement);
        Assert.Equal(MyMethodInIMyInterface, documentationElement.Text);
    }

    [Fact]
    public void GetTargetMember_NoCommentsInHierarchy_ShouldReturnNull()
    {
        XmlNode? node = null;
        var xdoc = new XDoc();
        var methodInfo = typeof(MyClassC).GetMethod(nameof(MyClassC.MethodA))!;

        var targetMember = InheritanceResolver.GetTargetMember(methodInfo, node);
        var documentationElement = xdoc.Get(targetMember!);

        Assert.Null(documentationElement);
    }

    [Fact]
    public void GetTargetMember_CommentsOnBothInterfaceAndBaseClass_ShouldPriorityBaseClass()
    {
        XmlNode? node = null;
        var xdoc = new XDoc();
        var methodInfo = typeof(MyClassWithMultipleInheritance).GetMethod(nameof(MyClassWithMultipleInheritance.MyMethod))!;

        var targetMember = InheritanceResolver.GetTargetMember(methodInfo, node);
        var documentationElement = xdoc.Get(targetMember!);

        Assert.NotNull(documentationElement);
        Assert.Equal(MyMethodInMyBaseClass, documentationElement.Text);
    }
    
    [Fact]
    public void GetTargetMember_CommentsOnBaseClass_ShouldPriorityBaseClass()
    {
        XmlNode? node = null;
        var xdoc = new XDoc();
        var type = typeof(MyClassWithMultipleInheritance);

        var targetMember = InheritanceResolver.GetTargetMember(type, node);
        var documentationElement = xdoc.Get(targetMember!);

        Assert.NotNull(documentationElement);
        Assert.Equal(CommentOnMyBaseClass, documentationElement.Text);
    }
    
    // [Fact]
    // public void GetTargetMember_CommentsOnBaseInterface_ShouldPriorityBaseClass()
    // {
    //     XmlNode? node = null;
    //     var xdoc = new XDoc();
    //     var type = typeof(MyClassC);
    //
    //     var targetMember = InheritanceResolver.GetTargetMember(type, node);
    //     var documentationElement = xdoc.Get(targetMember!);
    //
    //     Assert.NotNull(documentationElement);
    //     Assert.Equal(MyMethodInIMyInterface, documentationElement.Text);
    // }
}