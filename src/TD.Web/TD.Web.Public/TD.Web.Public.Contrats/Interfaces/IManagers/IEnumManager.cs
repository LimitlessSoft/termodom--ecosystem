using TD.Web.Common.Contracts.Dtos;

namespace TD.Web.Public.Contracts.Interfaces.IManagers;

public interface IEnumManager
{
	List<IdNamePairDto> GetProductStockTypes();
}
