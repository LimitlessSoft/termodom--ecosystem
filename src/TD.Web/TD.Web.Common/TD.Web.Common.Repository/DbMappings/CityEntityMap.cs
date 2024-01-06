using LSCore.Repository;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TD.Web.Common.Contracts.Entities;

namespace TD.Web.Common.Repository.DbMappings
{
    public class CityEntityMap : ILSCoreEntityMap<CityEntity>
    {
        public EntityTypeBuilder<CityEntity> Map(EntityTypeBuilder<CityEntity> entityTypeBuilder)
        {
            entityTypeBuilder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(32);

            return entityTypeBuilder;
        }
    }
}
