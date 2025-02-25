using System.Xml;
using TestAssembly.B;
using Xdoc.Renderer.PlainText;
using Xdoc.Renderer.PlaintText;

namespace BitzArt.XDoc.PlaintText.Tests;

public class PlainTextRendererTest
{
    [Fact]
    public async Task ToPlainText_CheckClass_ReturnNonEmptyResult()
    {
        var xDoc = new XDoc();
        var type = typeof(Dog);

        var str = xDoc.Get(type).ToPlainText();

        Assert.NotEmpty(str);
    }

    [Fact]
    public async Task ToPlainText_CheckProperty_ReturnNonEmptyResult()
    {
        var xDoc = new XDoc();
        var type = typeof(Dog);
        
        var field1Info = type.GetProperty(nameof(Dog.Field1));
        var field2Info = type.GetProperty(nameof(Dog.Field2));
        var nameInfo = type.GetProperty(nameof(Dog.Name));
        var idInfo = type.GetProperty(nameof(Dog.Id));

        var filed1Comment = xDoc.Get(field1Info!).ToPlainText();
        var filed2Comment = xDoc.Get(field2Info!).ToPlainText();
        var nameComment = xDoc.Get(nameInfo!).ToPlainText();
        var propertyDocumentation = xDoc.Get(idInfo!);
        var idComment = propertyDocumentation.ToPlainText();

        Assert.NotEmpty(filed1Comment);
        Assert.NotEmpty(filed2Comment);
        Assert.NotEmpty(nameComment);
        Assert.NotEmpty(idComment);
    }

    [Fact]
    public async Task ToPlainText_PropertyInfo()
    {
        var xDoc = new XDoc();
        var type = typeof(Dog);

        // var propertyInfo = type.GetProperty(nameof(Dog.Field1));
        var propertyInfo = type.GetProperty(nameof(Dog.Name));

        var propertyDocumentation = xDoc.Get(propertyInfo!)!;

        var str = propertyDocumentation.ToPlainText();
        Assert.NotEmpty(str);
    }


    [Fact]
    public async Task TextXmlRendering()
    {
        string xml = """
                     <summary>
                         Class B Name of specific <see cref="T:TestAssembly.B.Dog" />.
                         Not all <see cref="T:TestAssembly.A.Animal" />s can have a name.
                         <example>
                             Dog: Dog.Name = "Rex"
                         </example>
                         <remarks></remarks>
                         Be carefully with this property.
                     </summary>
                     """;

        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.LoadXml(xml);

        XmlNode xmlNode = xmlDoc.DocumentElement; // Get the root node

        var xmlRenderer = new XmlRenderer();
        var str = xmlRenderer.Render(xmlNode);

        Assert.NotEmpty(str);
    }

    [Fact]
    public async Task TextXmlRendering2()
    {
        string xml = """
                     <summary>
                         This method retrieves a specific <see cref="T:MyNamespace.MyClass" /> instance  based on the provided <paramref name="id" /> parameter.
                         
                         <para>
                             The method ensures that the returned instance is properly initialized 
                             and ready for use. If the <paramref name="id" /> is invalid, it will throw an 
                             <see cref="T:System.ArgumentException" />.
                         </para>
                     
                         <para>
                             Usage example:
                             <code>
                             var instance = MyClass.GetInstance(42);
                             Console.WriteLine(instance.Name);
                             </code>
                         </para>
                     
                         <list type="bullet">
                             <item><description>Ensures proper initialization.</description></item>
                             <item><description>Throws if <paramref name="id" /> is invalid.</description></item>
                             <item><description>Returns an instance of <typeparamref name="T" />.</description></item>
                         </list>
                     
                         <example>
                             The following example demonstrates how to use this method:
                             <code>
                             try
                             {
                                 var obj = MyClass.GetInstance(10);
                                 Console.WriteLine($"Object Name: {obj.Name}");
                             }
                             catch (ArgumentException ex)
                             {
                                 Console.WriteLine($"Error: {ex.Message}");
                             }
                             </code>
                         </example>
                     
                         <remarks>
                             Be careful when using this method with untrusted input, as passing an invalid 
                             <paramref name="id" /> may result in an exception.
                         </remarks>
                     </summary>
                     """;

        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.LoadXml(xml);

        XmlNode xmlNode = xmlDoc.DocumentElement; // Get the root node

        var xmlRenderer = new XmlRenderer();
        var str = xmlRenderer.Render(xmlNode);

        Assert.NotEmpty(str);
    }
}