using LSCore.Contracts.Dtos;

namespace TD.Web.Admin.Contracts.Interfaces.IManagers;

public interface IEnumManager
{
    List<LSCoreIdNamePairDto> GetOrderStatuses();
    List<LSCoreIdNamePairDto> GetUserTypes();
    List<LSCoreIdNamePairDto> GetProductGroupTypes();
    List<LSCoreIdNamePairDto> GetProductStockTypes();
}