namespace BitzArt.XDoc.PlainText.Tests;

class MyClass
{
    /// <summary>
    /// This is testing property from <see cref="MyClass"/>.
    /// </summary>
    public virtual int MyProperty { get; set; }
}

class MyInheritedClass : MyClass
{
    /// <inheritdoc />
    public override int MyProperty { get; set; }


    /// <summary>
    /// This is testing property from inherited class.
    /// </summary>
    public int AnotherProperty { get; set; }
}

class MyDoubleInheritedClass : MyInheritedClass
{
    /// <inheritdoc />
    public override int MyProperty { get; set; }
}

class MySimpleClass
{
    public int MyMethod() => 1;

    public int MyMethod(int x) => x + 1;
}

public class PlainTextRendererEndToEndTests
{
    private const string ThisIsTestingPropertyFromMyClass = "This is testing property from MyClass.";
    private const string ThisIsTestingPropertyFromMyClassWithNamespace = "This is testing property from BitzArt.XDoc.PlainText.Tests.MyClass.";
    private const string ThisIsTestingPropertyFromInheritedClass = "This is testing property from inherited class.";

    [Fact]
    public void ToPlainText_MyPropertyFromMyInheritedClass_ReturnsThisIsTestingPropertyFromMyClass()
    {
        // Arrange
        var xdoc = new XDoc();
        var propertyInfo = typeof(MyInheritedClass).GetProperty(nameof(MyInheritedClass.MyProperty));

        // Act
        var propertyDocumentation = xdoc.Get(propertyInfo!);
        var xmlComment = propertyDocumentation!.ToPlainText();

        // Assert
        Assert.Equal(ThisIsTestingPropertyFromMyClass, xmlComment);
    }

    [Fact]
    public void ToPlainText_MyPropertyFromMyDoubleInheritedClass_ReturnsThisIsTestingPropertyFromMyClass()
    {
        // Arrange
        var xdoc = new XDoc();
        var propertyInfo = typeof(MyDoubleInheritedClass).GetProperty(nameof(MyDoubleInheritedClass.MyProperty));

        // Act
        var propertyDocumentation = xdoc.Get(propertyInfo!);
        var xmlComment = propertyDocumentation!.ToPlainText();

        // Assert
        Assert.Equal(ThisIsTestingPropertyFromMyClass, xmlComment);
    }

    [Fact]
    public void ToPlainText_AnotherPropertyFromMyInheritedClass_ReturnsThisIsTestingPropertyFromInheritedClass()
    {
        // Arrange
        var xdoc = new XDoc();
        var propertyInfo = typeof(MyInheritedClass).GetProperty(nameof(MyInheritedClass.AnotherProperty));

        // Act
        var propertyDocumentation = xdoc.Get(propertyInfo!);
        var xmlComment = propertyDocumentation!.ToPlainText();

        // Assert
        Assert.Equal(ThisIsTestingPropertyFromInheritedClass, xmlComment);
    }

    [Fact]
    public void ToPlainText_MyMethodFromMySimpleClass_ReturnsEmptyString()
    {
        // Arrange
        var xdoc = new XDoc();
        var methodInfo = typeof(MySimpleClass).GetMethod(nameof(MySimpleClass.MyMethod), [typeof(int)])!;

        // Act
        var methodocumentation = xdoc.Get(methodInfo);

        // Assert
        Assert.Null(methodocumentation);
    }

    [Fact]
    public void ToPlainText_MyPropertyFromMyClass_ReturnsThisIsTestingPropertyFromMyClass()
    {
        // Arrange
        var xdoc = new XDoc();
        var propertyInfo = typeof(MyClass).GetProperty(nameof(MyClass.MyProperty));

        // Act
        var methodocumentation = xdoc.Get(propertyInfo!);
        var xmlComment = methodocumentation!.ToPlainText();

        // Assert
        Assert.Equal(ThisIsTestingPropertyFromMyClass, xmlComment);
    }
    
    [Fact]
    public void ToPlainText_MyPropertyFromMyDoubleInheritedClassWithNamespaces_ReturnsThisIsTestingPropertyFromMyClass()
    {
        // Arrange
        var xdoc = new XDoc();
        var propertyInfo = typeof(MyDoubleInheritedClass).GetProperty(nameof(MyDoubleInheritedClass.MyProperty));

        // Act
        var propertyDocumentation = xdoc.Get(propertyInfo!);
        var xmlComment = propertyDocumentation!.ToPlainText(options =>
        {
            options.RemoveNamespace = false;
        });

        // Assert
        Assert.Equal(ThisIsTestingPropertyFromMyClassWithNamespace, xmlComment);
    }
}