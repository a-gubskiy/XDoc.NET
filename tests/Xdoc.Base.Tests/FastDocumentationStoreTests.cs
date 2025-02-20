using TestAssembly;
using TestAssembly.A;
using TestAssembly.B;
using Xdoc.Renderer.PlaintText;

namespace Xdoc.Base.Tests;

public class FastDocumentationStoreTests
{
    [Fact]
    public async Task Test1()
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

        // Cause exception
        // var objectFriend = documentStore.GetPropertyInfo(typeof(Object), nameof(Animal.Friend));

        var renderer = new PlainTextRenderer(documentStore);

        var catClassInfoSummary = renderer.Render(catClassInfo);
        var dogClassInfoSummary = renderer.Render(dogClassInfo);

        var catNamePropertyInfoSummary = renderer.Render(catNamePropertyInfo);
        var blackCatNamePropertyInfoSummary = renderer.Render(blackCatNamePropertyInfo);
        var orangeCatNamePropertyInfoSummary = renderer.Render(orangeCatNamePropertyInfo);
        var dogNamePropertyInfoSummary = renderer.Render(dogNamePropertyInfo);
        var dogsFriendSummary = renderer.Render(dogsFriend);
        var animalFriendSummary = renderer.Render(animalFriend!);

        Assert.True(true);
    }
}