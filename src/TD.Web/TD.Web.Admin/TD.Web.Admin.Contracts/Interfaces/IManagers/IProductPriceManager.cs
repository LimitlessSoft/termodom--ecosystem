using LSCore.Common.Contracts;
using TD.Web.Admin.Contracts.Dtos.ProductPrices;
using TD.Web.Admin.Contracts.Requests.ProductsPrices;

namespace TD.Web.Admin.Contracts.Interfaces.IManagers;

public interface IProductPriceManager
{
	List<ProductsPricesGetDto> GetMultiple();
	void Delete(LSCoreIdRequest id);
	long Save(SaveProductPriceRequest request);
}
