using LSCore.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TD.Office.Common.Contracts.Entities;

namespace TD.Office.Common.Repository.EntityMappings
{
	public class OdsustvoEntityMap : LSCoreEntityMap<OdsustvoEntity>
	{
		public override Action<EntityTypeBuilder<OdsustvoEntity>> Mapper { get; } =
			builder =>
			{
				builder.Property(x => x.UserId).IsRequired();

				builder.Property(x => x.TipOdsustvaId).IsRequired();

				builder.Property(x => x.DatumOd).IsRequired();

				builder.Property(x => x.DatumDo).IsRequired();

				builder.Property(x => x.Komentar).HasMaxLength(500);

				builder
					.Property(x => x.Status)
					.IsRequired()
					.HasDefaultValueSql("0");

				builder.Property(x => x.OdobrenoAt);

				builder.Property(x => x.OdobrenoBy);

				builder
					.Property(x => x.RealizovanoKorisnik)
					.IsRequired()
					.HasDefaultValueSql("false");

				builder
					.Property(x => x.RealizovanoOdobravac)
					.IsRequired()
					.HasDefaultValueSql("false");

				builder
					.HasOne(x => x.User)
					.WithMany()
					.HasForeignKey(x => x.UserId);

				builder
					.HasOne(x => x.TipOdsustva)
					.WithMany()
					.HasForeignKey(x => x.TipOdsustvaId);

				builder
					.HasOne(x => x.OdobrenoByUser)
					.WithMany()
					.HasForeignKey(x => x.OdobrenoBy);
			};
	}
}
