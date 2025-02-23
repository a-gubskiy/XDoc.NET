using TestAssembly.B;

namespace BitzArt.XDoc.Tests;

public class XDocTests
{
    [Fact]
    public void GetDocumentation_ClassWithThreeFields_ShouldReturnMembersInfo()
    {
        var xDoc = new XDoc();

        var typeDocumentation = xDoc.GetDocumentation(typeof(Dog));

        var members = typeDocumentation.MemberData.Keys.ToList();

        Assert.Equal("Name", members[0].Name);
        Assert.Equal("Field1", members[1].Name);
        Assert.Equal("Field2", members[2].Name);
    }
}