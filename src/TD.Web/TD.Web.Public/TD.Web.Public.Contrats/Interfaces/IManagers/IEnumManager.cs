using LSCore.Contracts.Dtos;

namespace TD.Web.Public.Contracts.Interfaces.IManagers;

public interface IEnumManager
{
    List<LSCoreIdNamePairDto> GetProductStockTypes();
}