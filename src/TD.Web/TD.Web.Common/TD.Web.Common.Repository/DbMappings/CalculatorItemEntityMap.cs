using LSCore.Repository;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TD.Web.Common.Contracts.Entities;

namespace TD.Web.Common.Repository.DbMappings;

public class CalculatorItemEntityMap : LSCoreEntityMap<CalculatorItemEntity>
{
    public override Action<EntityTypeBuilder<CalculatorItemEntity>> Mapper { get; } = (builder) =>
    {
        builder.HasOne(x => x.Product)
            .WithMany();
    };
}