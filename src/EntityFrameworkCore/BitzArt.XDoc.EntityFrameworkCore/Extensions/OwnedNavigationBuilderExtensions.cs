using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BitzArt.XDoc;

public static class OwnedNavigationBuilderExtensions
{
    public static OwnedNavigationBuilder<TOwnerEntity, TDependentEntity> MapXmlComments<TOwnerEntity, TDependentEntity>(
        this OwnedNavigationBuilder<TOwnerEntity, TDependentEntity> builder)
        where TOwnerEntity : class
        where TDependentEntity : class
    {
        throw new NotImplementedException();
    }
}