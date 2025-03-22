using LSCore.Common.Extensions;
using TD.Web.Admin.Contracts.Interfaces.IManagers;
using TD.Web.Common.Contracts.Dtos;
using TD.Web.Common.Contracts.Enums;

namespace TD.Web.Admin.Domain.Managers;

public class EnumManager : IEnumManager
{
	public List<IdNamePairDto> GetOrderStatuses() =>
		Enum.GetValues<OrderStatus>()
			.Select(classification => new IdNamePairDto
			{
				Id = (int)classification,
				Name = classification.GetDescription()
			})
			.ToList();

	public List<IdNamePairDto> GetUserTypes() =>
		Enum.GetValues<UserType>()
			.Select(classification => new IdNamePairDto
			{
				Id = (int)classification,
				Name = classification.GetDescription()
			})
			.ToList();

	public List<IdNamePairDto> GetProductGroupTypes() =>
		Enum.GetValues<ProductGroupType>()
			.Select(classification => new IdNamePairDto
			{
				Id = (int)classification,
				Name = classification.GetDescription()
			})
			.ToList();

	public List<IdNamePairDto> GetProductStockTypes() =>
		Enum.GetValues<ProductStockType>()
			.Select(stockType => new IdNamePairDto
			{
				Id = (int)stockType,
				Name = stockType.GetDescription()
			})
			.ToList();

	public List<IdNamePairDto> GetCalculatorTypes() =>
		Enum.GetValues<CalculatorType>()
			.Select(calculatorType => new IdNamePairDto
			{
				Id = (int)calculatorType,
				Name = calculatorType.GetDescription()
			})
			.ToList();
}
