namespace BitzArt.XDoc.Tests;

/// <summary>
/// Test summary
/// </summary>
public class TestClass1
{
    /// <summary>
    /// Property
    /// </summary>
    public int MyProperty { get; set; }
}

/// <summary>
/// Test summary
/// </summary>
public class TestClass2
{
    public int MyProperty { get; set; }
}

public class TestClass3
{
    /// <summary>
    /// Property
    /// </summary>
    public int MyProperty { get; set; }
}

public class XmlUtilityTests
{
    [Fact]
    public async Task Test1()
    {
        var xdoc = new XDoc();

        var asmDocs = xdoc.GetDocumentation(GetType().Assembly);
    }
}