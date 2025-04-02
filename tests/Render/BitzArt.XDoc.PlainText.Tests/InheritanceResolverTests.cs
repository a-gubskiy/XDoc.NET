namespace BitzArt.XDoc.Tests;

class MyClassWithoutInheritance
{
    public int MyProperty => 1;
}

abstract class MyBaseClass
{
    public abstract string MyMethod();
}

class MyClassA : MyBaseClass
{
    public override string MyMethod() => "MyClassA.MyMethod";
}

class MyClassB : MyClassA
{
    public override string MyMethod() => "MyClassD.MyMethod";
}

interface IMyInterface1
{
    string MyMethod();
}

interface IMyInterface2
{
    string MyMethod();
}

class MyClassWithMultipleInterfaces : IMyInterface1, IMyInterface2
{
    public string MyMethod() => "MyClassWithMultipleInheritance.MyMethod";
}

public class InheritanceResolverTests
{
    [Fact]
    public void GetTargetMember_MethodOverrideFromBaseClass_ShouldReturnFromBaseClass()
    {
        // Arrange
        var methodInfo = typeof(MyClassB).GetMethod(nameof(MyClassB.MyMethod))!;
        var expected = typeof(MyClassA).GetMethod(nameof(MyClassA.MyMethod))!;

        // Act
        var targetMember = InheritanceResolver.GetTargetMember(methodInfo);

        // Assert
        Assert.Equal(expected, targetMember);
    }


    [Fact]
    public void GetTargetMember_MethodImplementedFromMultipleINterfaces_ShouldReturnFromFirstInterface()
    {
        // Arrange
        var methodInfo = typeof(MyClassWithMultipleInterfaces).GetMethod(nameof(MyClassWithMultipleInterfaces.MyMethod))!;
        var expected = typeof(IMyInterface1).GetMethod(nameof(IMyInterface1.MyMethod))!;

        // Act
        var targetMember = InheritanceResolver.GetTargetMember(methodInfo);

        // Assert
        Assert.Equal(expected, targetMember);
    }

    [Fact]
    public void GetTargetMember_PropertyImplementedOnlyItself_ShouldReturnNull()
    {
        // Arrange
        var methodInfo = typeof(MyClassWithoutInheritance).GetProperty(nameof(MyClassWithoutInheritance.MyProperty))!;

        // Act
        var targetMember = InheritanceResolver.GetTargetMember(methodInfo);

        // Assert
        Assert.Null(targetMember);
    }
}