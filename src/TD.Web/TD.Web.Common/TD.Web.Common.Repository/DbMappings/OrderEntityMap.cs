using LSCore.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;
using TD.Web.Common.Contracts.Entities;

namespace TD.Web.Common.Repository.DbMappings
{
    public class OrderEntityMap : LSCoreEntityMap<OrderEntity>
    {
        private readonly Int16 _noteMaxLength = 512;
        public override EntityTypeBuilder<OrderEntity> Map(EntityTypeBuilder<OrderEntity> entityTypeBuilder)
        {
            base.Map(entityTypeBuilder);

            entityTypeBuilder
                .Property(x => x.Status)
                .IsRequired();

            entityTypeBuilder
                .Property(x => x.Note)
                .HasMaxLength(_noteMaxLength);

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

            return entityTypeBuilder;
        }
    }
}
