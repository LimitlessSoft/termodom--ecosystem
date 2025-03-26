using LSCore.Common.Contracts;
using TD.Web.Admin.Contracts.Dtos.ProductsPricesGroup;
using TD.Web.Admin.Contracts.Requests.ProductPriceGroup;

namespace TD.Web.Admin.Contracts.Interfaces.IManagers;

public interface IProductPriceGroupManager
{
	long Save(ProductPriceGroupSaveRequest request);
	List<ProductPriceGroupGetDto> GetMultiple();
	void Delete(LSCoreIdRequest request);
}
