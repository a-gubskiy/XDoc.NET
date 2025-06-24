namespace BitzArt.XDoc.Tests;

public class XmlMemberNameResolverTests
{
    [Theory]
    [InlineData("", "")]
    [InlineData("`", "")]
    [InlineData("NameSpace.MyClass`", "NameSpace.MyClass")]
    [InlineData("NameSpace.MyClass`1", "NameSpace.MyClass")]
    [InlineData("NameSpace.MyClass`.MyMethod", "NameSpace.MyClass.MyMethod")]
    [InlineData("NameSpace.MyClass`1.MyMethod", "NameSpace.MyClass.MyMethod")]
    public void RemoveGenericMarkers_OnValidInput_ShouldReplace(string input, string expected)
    {
        // Act
        var result = XmlMemberNameResolver.RemoveGenericMarkers(input);

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void ResolveTypeAndMemberName_GenericMethod_ShouldResolve()
    {
        // Arrange
        var value = "RootNameSpace.ChildNameSpace.MyClass`1.MyMethod(`0)";

        // Act
        var (type, memberName) = XmlMemberNameResolver.ResolveTypeAndMemberName(value);

        // Assert
        Assert.Equal("RootNameSpace.ChildNameSpace.MyClass`1", type);
        Assert.Equal("MyMethod", memberName);
    }
    
    [Fact]
    public void ResolveTypeAndMemberName_GenericMethodWithFewParameters_ShouldResolve()
    {
        // Arrange
        var value = "RootNameSpace.ChildNameSpace.MyExtension.SomeMethod``1(``0,``0)";

        // Act
        var (type, memberName) = XmlMemberNameResolver.ResolveTypeAndMemberName(value);

        // Assert
        Assert.Equal("RootNameSpace.ChildNameSpace.MyExtension", type);
        Assert.Equal("SomeMethod", memberName);
    }
    
    [Fact]
    public void ResolveTypeAndMemberName_GenericMethodWithComplexParameter_ShouldResolve()
    {
        // Arrange
        var value = "RootNameSpace.ChildNameSpace.MyExtension.MyProperty``1(System.Linq.IQueryable{``0},System.Int32)";

        // Act
        var (type, memberName) = XmlMemberNameResolver.ResolveTypeAndMemberName(value);

        // Assert
        Assert.Equal("RootNameSpace.ChildNameSpace.MyExtension", type);
        Assert.Equal("MyProperty", memberName);
    } 
    
    [Fact]
    public void ResolveMethodParameters_GenericMethodWithComplexParameters_ShouldResolve()
    {
        // Arrange
        var value = "RootNameSpace.ChildNameSpace.SomeClass`1.SomeMethod``1(System.Func{System.Linq.IQueryable{`0},System.Linq.IQueryable{``0}},System.Threading.CancellationToken)";
        
        // Act
        var parameters = XmlMemberNameResolver.ResolveMethodParameters(value);

        // Assert
        Assert.Equal(2, parameters.Count);
    }
    
    [Fact]
    public void ResolveMethodParameters_GenericMethodWithNineComplexParameters_ShouldResolve()
    {
        // Arrange
        var value = "M:RootNameSpace.ChildNameSpace.SomeClass`1.Method(System.Linq.IQueryable{System.String},System.Nullable{System.Int64},System.Nullable{System.Int32},System.Boolean,System.String,System.Collections.Generic.IEnumerable{System.String},System.String,System.String,System.Boolean)";
        
        // Act
        var parameters = XmlMemberNameResolver.ResolveMethodParameters(value);

        // Assert
        Assert.Equal(9, parameters.Count);
    }
}