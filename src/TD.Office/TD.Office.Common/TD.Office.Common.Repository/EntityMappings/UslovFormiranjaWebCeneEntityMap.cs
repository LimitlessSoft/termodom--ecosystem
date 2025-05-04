using LSCore.Repository;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TD.Office.Common.Contracts.Entities;

namespace TD.Office.Common.Repository.EntityMappings
{
	public class UslovFormiranjaWebCeneEntityMap : LSCoreEntityMap<UslovFormiranjaWebCeneEntity>
	{
		public override Action<EntityTypeBuilder<UslovFormiranjaWebCeneEntity>> Mapper { get; } =
			builder => { };
	}
}
