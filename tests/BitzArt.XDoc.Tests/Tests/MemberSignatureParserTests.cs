namespace BitzArt.XDoc.Tests;

public class MemberSignatureParserTests
{
    [Fact]
    public void ResolveTypeAndMemberName_Generic_ShouldParse()
    {
        // Arrange

        // Act
        var value = "BitzArt.CA.IRepository`1.Add(`0)";
        var (type, memberName) = MemberSignatureParser.ResolveTypeAndMemberName(value);

        // Assert
        Assert.Equal("BitzArt.CA.IRepository`1", type);
        Assert.Equal("Add", memberName);
    }
    
    [Fact]
    public void ResolveTypeAndMemberName2_Generic_ShouldParse()
    {
        // Arrange

        // Act
        var value = "MediaMars.Management.IsNewerExtension.IsNewer``1(``0,``0)";
        var (type, memberName) = MemberSignatureParser.ResolveTypeAndMemberName(value);

        // Assert
        Assert.Equal("MediaMars.Management.IsNewerExtension", type);
        Assert.Equal("IsNewer", memberName);
    }
    
    [Fact]
    public void ResolveTypeAndMemberName3_Generic_ShouldParse()
    {
        // Arrange

        // Act
        var value = "MediaMars.Management.SiteOwnedQueryExtensions.WithSiteId``1(System.Linq.IQueryable{``0},System.Int32)";
        var (type, memberName) = MemberSignatureParser.ResolveTypeAndMemberName(value);

        // Assert
        Assert.Equal("MediaMars.Management.SiteOwnedQueryExtensions", type);
        Assert.Equal("WithSiteId", memberName);
    } 
    
    [Fact]
    public void ResolveMethodParameters_Generic_ShouldReturnCorrectCount()
    {
        var value = "BitzArt.CA.IRepository`1.GetAllAsync``1(System.Func{System.Linq.IQueryable{`0},System.Linq.IQueryable{``0}},System.Threading.CancellationToken)";
        var parameters = MemberSignatureParser.ResolveMethodParameters(value);

        // Assert
        Assert.Equal(2, parameters.Count);
    }
}