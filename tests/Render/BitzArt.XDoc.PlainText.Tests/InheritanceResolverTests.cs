using System.Xml;

namespace BitzArt.XDoc.Tests;

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

interface IMyInterface1 : IMyInterface
{
    int MethodA();
}

interface IMyInterface2
{
    int MethodB();
}

class MyClassC : IMyInterface1, IMyInterface2
{
    public string MyMethod() => "MyClassC.MyMethod";

    public int MethodA() => 0;

    public int MethodB() => 0;
}

class MyClassD : MyClassA
{
}

public class InheritanceResolverTests
{
    private const string MyMethodInIMyInterface = "My method in IMyInterface";
    private const string MyMethodInMyBaseClass = "My method in MyBaseClass";

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
}