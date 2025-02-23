using TestAssembly.A;
using TestAssembly.B;
using Xdoc.Abstractions;

namespace Xdoc.Renderer.PlaintText.Tests;

public class UnitTest1
{
    [Fact(Skip = "Rendering is not implemented yet.")]
    public async Task TestRendering()
    {
        IDocumentStore documentStore = new DocumentStore();

        var catClassInfo = documentStore.GetClassInfo(typeof(Cat));
        var dogClassInfo = documentStore.GetClassInfo(typeof(Dog));

        var catNamePropertyInfo = documentStore.GetPropertyInfo(typeof(Cat), nameof(Cat.Name));

        var blackCatNamePropertyInfo = documentStore.GetPropertyInfo(typeof(BlackCat), nameof(BlackCat.Name));
        var orangeCatNamePropertyInfo = documentStore.GetPropertyInfo(typeof(OrangeCat), nameof(OrangeCat.Name));

        var dogNamePropertyInfo = documentStore.GetPropertyInfo(typeof(Dog), nameof(Dog.Name));

        var dogsFriend = documentStore.GetPropertyInfo(typeof(Dog), nameof(Dog.Friend));
        var animalFriend = documentStore.GetPropertyInfo(typeof(Animal), nameof(Animal.Friend));

        var renderer = new PlainTextRenderer(documentStore);

        var catClassInfoSummary = renderer.Render(catClassInfo);
        var dogClassInfoSummary = renderer.Render(dogClassInfo);
        var catNamePropertyInfoSummary = renderer.Render(catNamePropertyInfo);
        var blackCatNamePropertyInfoSummary = renderer.Render(blackCatNamePropertyInfo);
        var orangeCatNamePropertyInfoSummary = renderer.Render(orangeCatNamePropertyInfo);
        var dogNamePropertyInfoSummary = renderer.Render(dogNamePropertyInfo);
        var dogsFriendSummary = renderer.Render(dogsFriend);
        var animalFriendSummary = renderer.Render(animalFriend!);

        Assert.False(string.IsNullOrWhiteSpace(catClassInfoSummary));
        Assert.False(string.IsNullOrWhiteSpace(dogClassInfoSummary));
        Assert.False(string.IsNullOrWhiteSpace(catNamePropertyInfoSummary));
        Assert.False(string.IsNullOrWhiteSpace(blackCatNamePropertyInfoSummary));
        Assert.False(string.IsNullOrWhiteSpace(orangeCatNamePropertyInfoSummary));
        Assert.False(string.IsNullOrWhiteSpace(dogNamePropertyInfoSummary));
        Assert.False(string.IsNullOrWhiteSpace(dogsFriendSummary));
        Assert.False(string.IsNullOrWhiteSpace(animalFriendSummary));
    }
}