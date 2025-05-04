using LSCore.Repository;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TD.Web.Common.Contracts.Entities;

namespace TD.Web.Common.Repository.DbMappings
{
	public class KomercijalnoWebProductLinkEntityMap
		: LSCoreEntityMap<KomercijalnoWebProductLinkEntity>
	{
		public override Action<
			EntityTypeBuilder<KomercijalnoWebProductLinkEntity>
		> Mapper { get; } =
			entityTypeBuilder =>
			{
				entityTypeBuilder.HasIndex(x => new { x.RobaId, x.WebId }).IsUnique();
			};
	}
}
