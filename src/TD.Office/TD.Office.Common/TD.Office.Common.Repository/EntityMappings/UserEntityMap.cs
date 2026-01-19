using LSCore.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TD.Office.Common.Contracts.Entities;

namespace TD.Office.Common.Repository.EntityMappings
{
	public class UserEntityMap : LSCoreEntityMap<UserEntity>
	{
		public override Action<EntityTypeBuilder<UserEntity>> Mapper { get; } =
			builder =>
			{
				builder.Property(x => x.Username).IsRequired().HasMaxLength(64);

				builder.Property(x => x.Password).IsRequired().HasMaxLength(64);

				builder
					.HasMany(x => x.Permissions)
					.WithOne(x => x.User)
					.HasForeignKey(x => x.UserId);

				builder.Property(x => x.MaxRabatMPDokumenti).HasDefaultValue(5);

				builder.Property(x => x.MaxRabatVPDokumenti).HasDefaultValue(5);

				builder
					.HasOne(x => x.TipKorisnika)
					.WithMany()
					.HasForeignKey(x => x.TipKorisnikaId)
					.OnDelete(Microsoft.EntityFrameworkCore.DeleteBehavior.SetNull);
			};
	}
}
