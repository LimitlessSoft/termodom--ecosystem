using TD.Web.Common.Contracts.Dtos;

namespace TD.Web.Admin.Contracts.Interfaces.IManagers;

public interface IEnumManager
{
	List<IdNamePairDto> GetOrderStatuses();
	List<IdNamePairDto> GetUserTypes();
	List<IdNamePairDto> GetProductGroupTypes();
	List<IdNamePairDto> GetProductStockTypes();
	List<IdNamePairDto> GetCalculatorTypes();
}
