using LSCore.Common.Extensions;
using TD.Web.Common.Contracts.Dtos;
using TD.Web.Common.Contracts.Enums;
using TD.Web.Public.Contracts.Interfaces.IManagers;

namespace TD.Web.Public.Domain.Managers;

public class EnumManager : IEnumManager
{
	public List<IdNamePairDto> GetProductStockTypes() =>
		Enum.GetValues<ProductStockType>()
			.Select(stockType => new IdNamePairDto
			{
				Id = (int)stockType,
				Name = stockType.GetDescription()
			})
			.ToList();
}
