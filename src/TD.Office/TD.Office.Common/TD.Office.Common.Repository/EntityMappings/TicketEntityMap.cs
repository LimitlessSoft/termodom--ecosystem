using LSCore.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TD.Office.Common.Contracts.Entities;

namespace TD.Office.Common.Repository.EntityMappings
{
	public class TicketEntityMap : LSCoreEntityMap<TicketEntity>
	{
		public override Action<EntityTypeBuilder<TicketEntity>> Mapper { get; } =
			builder =>
			{
				builder.Property(x => x.Title).IsRequired().HasMaxLength(200);

				builder.Property(x => x.Description).IsRequired().HasMaxLength(4000);

				builder
					.Property(x => x.Type)
					.IsRequired()
					.HasDefaultValueSql("0");

				builder
					.Property(x => x.Status)
					.IsRequired()
					.HasDefaultValueSql("0");

				builder
					.Property(x => x.Priority)
					.IsRequired()
					.HasDefaultValueSql("1");

				builder.Property(x => x.SubmittedByUserId).IsRequired();

				builder.Property(x => x.DeveloperNotes).HasMaxLength(4000);

				builder.Property(x => x.ResolvedAt);

				builder.Property(x => x.ResolvedByUserId);

				builder.Property(x => x.ResolutionNotes).HasMaxLength(2000);

				builder
					.HasOne(x => x.SubmittedByUser)
					.WithMany()
					.HasForeignKey(x => x.SubmittedByUserId);

				builder
					.HasOne(x => x.ResolvedByUser)
					.WithMany()
					.HasForeignKey(x => x.ResolvedByUserId);
			};
	}
}
