using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TD.Core.Repository;
using TD.Komercijalno.Contracts.Entities;

namespace TD.Komercijalno.Repository.DbMappings
{
    public class NamenaMap : IEntityMap<Namena>
    {
        public EntityTypeBuilder<Namena> Map(EntityTypeBuilder<Namena> entityTypeBuilder)
        {
            entityTypeBuilder
                .HasKey(x => x.Id);

            return entityTypeBuilder;
        }
    }
}
