namespace BitzArt.XDoc.Tests;

public interface IA
{
    /// <summary>
    /// This is a method
    /// </summary>
    /// <returns></returns>
    string MethodOne();
}

/// <summary>
/// Test comment for the type
/// </summary>
public interface IB
{
    string MethodOne();
}

/// <summary>
/// Test comment for the type
/// </summary>
public interface IC
{
    /// <summary>
    /// This is another method
    /// </summary>
    /// <returns></returns>
    string MethodOne();
    
    string MethodThree();
}

public class BaseClass : IA, IC
{
    public string MethodOne() => "Method one";

    public string MethodTwo() => "Method two";
    
    /// <summary>
    /// This is third method
    /// </summary>
    /// <returns></returns>
    public string MethodThree() => "Method three";
}

public class TestingClass : BaseClass, IB
{
}

public class InheritanceResolverTests
{
    [Fact]
    public void GetTypeInheritedComment_CommentOnParentInterface_ShouldReturnComment()
    {
        var resolver = new InheritanceResolver(new XDoc());

        var type = typeof(TestingClass);
        var documentationElement = resolver.GetDocumentationElement(type);

        Assert.NotNull(documentationElement);
        Assert.Equal("Test comment for the type", documentationElement.Node.InnerText.Trim());
    }

    [Fact]
    public void GetMemberInheritedComment_CommentOnParentInterface_ShouldReturnComment()
    {
        var resolver = new InheritanceResolver(new XDoc());
        var type = typeof(TestingClass);
        var memberInfo = type.GetMember(nameof(TestingClass.MethodOne)).First();

        var documentationElement = resolver.GetDocumentationElement(memberInfo);

        Assert.NotNull(documentationElement);
        Assert.Equal("This is a method", documentationElement.Node.InnerText.Trim());
    }
    
    [Fact]
    public void GetMemberInheritedComment_CommentOnParentClass_ShouldReturnComment()
    {
        var resolver = new InheritanceResolver(new XDoc());
        var type = typeof(TestingClass);
        var memberInfo = type.GetMember(nameof(TestingClass.MethodThree)).First();

        var documentationElement = resolver.GetDocumentationElement(memberInfo);

        Assert.NotNull(documentationElement);
        Assert.Equal("This is third method", documentationElement.Node.InnerText.Trim());
    }
}