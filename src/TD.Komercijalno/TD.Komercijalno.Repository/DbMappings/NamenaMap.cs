using LSCore.Repository;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TD.Komercijalno.Contracts.Entities;

namespace TD.Komercijalno.Repository.DbMappings
{
    public class NamenaMap : ILSCoreEntityMap<Namena>
    {
        public EntityTypeBuilder<Namena> Map(EntityTypeBuilder<Namena> entityTypeBuilder)
        {
            entityTypeBuilder
                .HasKey(x => x.Id);

            return entityTypeBuilder;
        }
    }
}
