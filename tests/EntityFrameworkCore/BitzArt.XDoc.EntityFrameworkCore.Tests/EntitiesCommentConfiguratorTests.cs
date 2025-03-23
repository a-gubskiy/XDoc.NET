using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;

namespace BitzArt.XDoc.Tests;

public class EntitiesCommentConfiguratorTests
{
    private class TestEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    private class TestDbContext1(Action<TestDbContext, ModelBuilder> onModelCreating) : TestDbContext(onModelCreating);

    [Fact]
    public void ConfigureComments_SetsCommentsOnNonOwnedEntities()
    {
        // Arrange
        var xDoc = new XDoc();

        var testContext = new TestDbContext1((context, modelBuilder) =>
        {
            modelBuilder.Entity<MyFirstClass>();

            var configurator = new EntitiesCommentConfigurator(xDoc);

            configurator.ConfigureComments(modelBuilder);
        });

        // Act
        _ = testContext.Model;

        var designTimeModel = testContext.GetService<IDesignTimeModel>();
        var model = designTimeModel.Model;

        var entityType = model.FindEntityType(typeof(MyFirstClass));
        var property = entityType.FindProperty(nameof(MyFirstClass.Id));
        var comment = property.GetComment();

        Assert.Equal("Id Comment", comment);
    }
}