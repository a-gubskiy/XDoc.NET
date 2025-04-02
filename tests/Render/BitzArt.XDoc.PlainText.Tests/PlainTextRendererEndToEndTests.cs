namespace BitzArt.XDoc.Tests;

class MyClass
{
    /// <summary>
    /// This is testing property from MyClass.
    /// </summary>
    public virtual int TestPropertyFromMyClass { get; set; }
}

class MyInheritedClass : MyClass
{
    /// <inheritdoc />
    public override int TestPropertyFromMyClass { get; set; }


    /// <summary>
    /// This is testing property from inherited class.
    /// </summary>
    public int TestPropertyFromMyInheritedClass { get; set; }
}

class MyDoubleInheritedClass : MyInheritedClass
{
    /// <inheritdoc />
    public override int TestPropertyFromMyClass { get; set; }
}

class MySimpleClass
{
    public int MyMethod() => 1;
}

public class PlainTextRendererEndToEndTests
{
    private const string ThisIsTestingPropertyFromMyClass = "This is testing property from MyClass.";
    private const string ThisIsTestingPropertyFromInheritedClass = "This is testing property from inherited class.";

    [Fact]
    public void Render_ExtractsPropertySummary_ForMyInheritedClassReturnXmlCommentFromMyClass()
    {
        // Arrange
        var xdoc = new XDoc();
        var propertyInfo = typeof(MyInheritedClass).GetProperty(nameof(MyInheritedClass.TestPropertyFromMyClass));

        // Act
        var propertyDocumentation = xdoc.Get(propertyInfo);
        var xmlComment = propertyDocumentation.ToPlainText();

        // Assert
        Assert.Equal(ThisIsTestingPropertyFromMyClass, xmlComment);
    }

    [Fact]
    public void Render_ExtractsPropertySummary_ForMyDoubleInheritedClassReturnXmlCommentFromMyClass()
    {
        // Arrange
        var xdoc = new XDoc();
        var propertyInfo =
            typeof(MyDoubleInheritedClass).GetProperty(nameof(MyDoubleInheritedClass.TestPropertyFromMyClass));

        // Act
        var propertyDocumentation = xdoc.Get(propertyInfo);
        var xmlComment = propertyDocumentation.ToPlainText();

        // Assert
        Assert.Equal(ThisIsTestingPropertyFromMyClass, xmlComment);
    }

    [Fact]
    public void Render_ExtractsPropertySummary_ForMyInheritedClassReturnXmlCommentFromMyInheritedClass()
    {
        // Arrange
        var xdoc = new XDoc();
        var propertyInfo = typeof(MyInheritedClass).GetProperty(nameof(MyInheritedClass.TestPropertyFromMyInheritedClass));

        // Act
        var propertyDocumentation = xdoc.Get(propertyInfo);
        var xmlComment = propertyDocumentation.ToPlainText();

        // Assert
        Assert.Equal(ThisIsTestingPropertyFromInheritedClass, xmlComment);
    }

    [Fact]
    public void Render_ExtractsPropertySummary_ForMySimpleClassReturnEmpty()
    {
        // Arrange
        var xdoc = new XDoc();
        var methodInfo = typeof(MySimpleClass).GetMethod(nameof(MySimpleClass.MyMethod));

        // Act
        var methodocumentation = xdoc.Get(methodInfo);
        var xmlComment = methodocumentation.ToPlainText();

        // Assert
        Assert.Equal(string.Empty, xmlComment);
    }  
    
    [Fact]
    public void Render_ExtractsPropertySummary_ForMyClassReturnComment()
    {
        // Arrange
        var xdoc = new XDoc();
        var propertyInfo = typeof(MyClass).GetProperty(nameof(MyClass.TestPropertyFromMyClass));

        // Act
        var methodocumentation = xdoc.Get(propertyInfo);
        var xmlComment = methodocumentation.ToPlainText();

        // Assert
        Assert.Equal(ThisIsTestingPropertyFromMyClass, xmlComment);
    }
}