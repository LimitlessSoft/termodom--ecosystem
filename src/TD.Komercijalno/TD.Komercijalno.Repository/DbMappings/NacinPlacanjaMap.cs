using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TD.Core.Repository;
using TD.Komercijalno.Contracts.Entities;

namespace TD.Komercijalno.Repository.DbMappings
{
    public class NacinPlacanjaMap : IEntityMap<NacinPlacanja>
    {
        public EntityTypeBuilder<NacinPlacanja> Map(EntityTypeBuilder<NacinPlacanja> entityTypeBuilder)
        {
            entityTypeBuilder
                .HasKey(x => x.Id);

            return entityTypeBuilder;
        }
    }
}
