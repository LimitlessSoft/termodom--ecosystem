using LSCore.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TD.Web.Common.Contracts.Entities;
using TD.Web.Common.Contracts.Enums;

namespace TD.Web.Common.Repository.DbMappings
{
	public class BlogEntityMap : LSCoreEntityMap<BlogEntity>
	{
		private const int TitleMaxLength = 256;
		private const int SlugMaxLength = 256;
		private const int SummaryMaxLength = 512;

		public override Action<EntityTypeBuilder<BlogEntity>> Mapper { get; } =
			entityTypeBuilder =>
			{
				entityTypeBuilder.Property(x => x.Title).IsRequired().HasMaxLength(TitleMaxLength);

				entityTypeBuilder.Property(x => x.Text).IsRequired();

				entityTypeBuilder.Property(x => x.Slug).IsRequired().HasMaxLength(SlugMaxLength);
				entityTypeBuilder.HasIndex(x => x.Slug).IsUnique();

				entityTypeBuilder.Property(x => x.Status)
					.IsRequired()
					.HasDefaultValue(BlogStatus.Draft);

				entityTypeBuilder.Property(x => x.Summary).HasMaxLength(SummaryMaxLength);
			};
	}
}
