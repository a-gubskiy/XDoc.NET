using System.Reflection;
using System.Xml;

namespace BitzArt.XDoc.Tests;

public class TestClass
{
    /// <summary>
    /// Test property documentation.
    /// </summary>
    public string TestProperty { get; set; }

    /// <summary>
    /// Test method documentation.
    /// </summary>
    public void TestMethod()
    {
    }

    /// <summary>
    /// Test field documentation.
    /// </summary>
    public string TestField;
}

public class XDocGetMemberDocumentationTests
{
    [Fact]
    public void GetProperty_ReturnsPropertyDocumentation()
    {
        // Arrange
        var xdoc = new XDoc();
        var property = typeof(TestClass).GetProperty(nameof(TestClass.TestProperty));

        // Act
        var result = xdoc.Get(property!);

        // Assert
        Assert.NotNull(result);
    }

    [Fact]
    public void GetMethod_ReturnsMethodDocumentation()
    {
        // Arrange
        var xdoc = new XDoc();
        var method = typeof(TestClass).GetMethod(nameof(TestClass.TestMethod));

        // Act
        var result = xdoc.Get(method!);

        // Assert
        Assert.NotNull(result);
    }

    [Fact]
    public void GetField_ReturnsFieldDocumentation()
    {
        // Arrange
        var xdoc = new XDoc();
        var field = typeof(TestClass).GetField(nameof(TestClass.TestField));

        // Act
        var result = xdoc.Get(field)!;

        // Assert
        Assert.NotNull(result);
    }

    [Fact]
    public void GetMember_PropertyAsMember_ReturnsPropertyDocumentation()
    {
        // Arrange
        var xdoc = new XDoc();
        var member = typeof(TestClass).GetProperty(nameof(TestClass.TestProperty));

        // Act
        var result = xdoc.Get(member);

        // Assert
        Assert.NotNull(result);
        Assert.IsType<PropertyDocumentation>(result);
    }

    [Fact]
    public void GetMember_MethodAsMember_ReturnsMethodDocumentation()
    {
        // Arrange
        var xdoc = new XDoc();
        var member = typeof(TestClass).GetMethod(nameof(TestClass.TestMethod));

        // Act
        var result = xdoc.Get(member);

        // Assert
        Assert.NotNull(result);
        Assert.IsType<MethodDocumentation>(result);
    }

    [Fact]
    public void GetMember_FieldAsMember_ReturnsFieldDocumentation()
    {
        // Arrange
        var xdoc = new XDoc();
        var member = typeof(TestClass).GetField(nameof(TestClass.TestField));

        // Act
        var result = xdoc.Get(member);

        // Assert
        Assert.NotNull(result);
        Assert.IsType<FieldDocumentation>(result);
    }

    [Fact]
    public void GetProperty_PropertyWithoutDocumentation_ReturnsNull()
    {
        // Arrange
        var xdoc = new XDoc();
        var property = typeof(string).GetProperty(nameof(string.Length));

        // Act
        var result = xdoc.Get(property);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void GetMember_NullMember_ThrowsArgumentNullException()
    {
        // Arrange
        var xdoc = new XDoc();

        MemberInfo member = null;

        // Assert
        Assert.Throws<NullReferenceException>(() =>
        {
            var memberDocumentation = xdoc.Get(member);

            return memberDocumentation;
        });
    }

    [Fact]
    public void GetMember_NullMember_ThrowsXDocException()
    {
        // Arrange
        var xml = @"<?xml version=""1.0""?>
            <doc>
                <assembly><name>BitzArt.XDoc.Tests</name></assembly>
            </doc>";

        var type = typeof(TestClass);
        
        var assembly = Assembly.GetAssembly(type);

        var doc = new XmlDocument();
        doc.LoadXml(xml);
        var xdoc = new XDoc();


        // Assert
        Assert.Throws<XDocException>(() => { XmlUtility.Fetch(doc, xdoc, assembly); });
    }
}