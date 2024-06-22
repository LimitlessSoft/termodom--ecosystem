using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TD.Web.Common.Contracts.Entities;
using LSCore.Repository;

namespace TD.Web.Common.Repository.DbMappings
{
    public class OrderEntityMap : LSCoreEntityMap<OrderEntity>
    {
        private const int NoteMaxLength = 512;

        public override Action<EntityTypeBuilder<OrderEntity>> Mapper { get; } = entityTypeBuilder =>
        {
            entityTypeBuilder
                .Property(x => x.Status)
                .IsRequired();

            entityTypeBuilder
                .Property(x => x.Note)
                .HasMaxLength(NoteMaxLength);

            entityTypeBuilder
                .HasOne(x => x.Referent)
                .WithMany()
                .HasForeignKey(x => x.ReferentId);

            entityTypeBuilder
                .HasOne(x => x.User)
                .WithMany()
                .HasForeignKey(x => x.CreatedBy);

            entityTypeBuilder
                .HasOne(x => x.PaymentType)
                .WithMany()
                .HasForeignKey(x => x.PaymentTypeId);

            entityTypeBuilder
                .HasMany(x => x.Items)
                .WithOne(x => x.Order)
                .HasForeignKey(x => x.OrderId);
        };
    }
}
