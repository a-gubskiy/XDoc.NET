using System.Xml;

namespace BitzArt.XDoc.Tests;

class MyBaseClass
{
    /// <summary>
    /// My method in MyBaseClass
    /// </summary>
    public virtual void MyMethod()
    {
    }
}

class MyClassA : MyBaseClass
{
    public override void MyMethod()
    {
    }
}

interface IMyInterface
{
    /// <summary>
    /// My method in IMyInterface
    /// </summary>
    void MyMethod();
}

class MyClassB : IMyInterface
{
    public void MyMethod()
    {
    }
}

interface IMyInterface1 : IMyInterface
{
    void MethodA();
}

interface IMyInterface2
{
    void MethodB();
}

class MyClassC : IMyInterface1, IMyInterface2
{
    public void MyMethod()
    {
    }

    public void MethodA()
    {
        throw new NotImplementedException();
    }

    public void MethodB()
    {
        throw new NotImplementedException();
    }
}

public class InheritanceResolverTests
{
    [Fact]
    public void GetTargetMember_CommentOnMethodInBaseClass_ShouldReturnComment()
    {
        XmlNode? node = null;
        var xdoc = new XDoc();
        var methodInfo = typeof(MyClassA).GetMethod(nameof(MyClassA.MyMethod))!;
        
        var targetMember = InheritanceResolver.GetTargetMember(methodInfo, node);
        var documentationElement = xdoc.Get(targetMember!);
        
        Assert.NotNull(documentationElement);
        Assert.Equal("My method in MyBaseClass", documentationElement.Text);
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
        Assert.Equal("My method in IMyInterface", documentationElement.Text);
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
        Assert.Equal("My method in IMyInterface", documentationElement.Text);
    }
    
    // [Fact]
    // public void GetTargetMember_CommentOnMethodInParentClass_ShouldReturnComment()
    // {
    //     XmlNode? node = null;
    //     var xdoc = new XDoc();
    //     var type = typeof(TestingClass);
    //     var memberInfo = type.GetMember(nameof(TestingClass.MethodThree)).First();
    //
    //     // Resolve the target member from the given member info.
    //     var targetMember = InheritanceResolver.GetTargetMember(memberInfo, node);
    //     var documentationElement = xdoc.Get(targetMember);
    //
    //     Assert.NotNull(documentationElement);
    //     Assert.Equal("This is third method", documentationElement.Node.InnerText.Trim());
    // }
    //
    //
    // [Fact]
    // public void GetTargetMember_CommentOnMethodInParentInterface_ShouldReturnComment()
    // {
    //     XmlNode? node = null;
    //     var xdoc = new XDoc();
    //     var type = typeof(TestingClass);
    //     var memberInfo = type.GetMember(nameof(TestingClass.MethodOne)).First();
    //     
    //     var targetMember = InheritanceResolver.GetTargetMember(memberInfo, node);
    //     var documentationElement = xdoc.Get(targetMember);
    //
    //     Assert.NotNull(documentationElement);
    //     Assert.Equal("This is a method", documentationElement.Node.InnerText.Trim());
    // }
    //
    // [Fact]
    // public void GetTargetMember_CommentOnParentInterface_ShouldReturnComment()
    // {
    //     XmlNode? node = null;
    //     var xdoc = new XDoc();
    //     var type = typeof(TestingClass);
    //     // var inheritdoc = CreateInheritdocElement();
    //
    //     var targetMember = InheritanceResolver.GetTargetMember(type, node);
    //     var documentationElement = xdoc.Get(targetMember);
    //
    //     Assert.NotNull(documentationElement);
    //     Assert.Equal("Test comment for the type", documentationElement.Node.InnerText.Trim());
    // }
}