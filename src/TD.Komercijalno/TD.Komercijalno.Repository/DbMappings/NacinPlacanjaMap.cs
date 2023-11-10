using LSCore.Repository;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TD.Komercijalno.Contracts.Entities;

namespace TD.Komercijalno.Repository.DbMappings
{
    public class NacinPlacanjaMap : ILSCoreEntityMap<NacinPlacanja>
    {
        public EntityTypeBuilder<NacinPlacanja> Map(EntityTypeBuilder<NacinPlacanja> entityTypeBuilder)
        {
            entityTypeBuilder
                .HasKey(x => x.Id);

            return entityTypeBuilder;
        }
    }
}
