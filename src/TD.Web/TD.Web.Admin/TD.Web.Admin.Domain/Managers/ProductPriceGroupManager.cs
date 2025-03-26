using LSCore.Common.Contracts;
using LSCore.Mapper.Domain;
using Omu.ValueInjecter;
using TD.Web.Admin.Contracts.Dtos.ProductsPricesGroup;
using TD.Web.Admin.Contracts.Interfaces.IManagers;
using TD.Web.Admin.Contracts.Requests.ProductPriceGroup;
using TD.Web.Common.Contracts.Entities;
using TD.Web.Common.Contracts.Interfaces.IRepositories;

namespace TD.Web.Admin.Domain.Managers;

public class ProductPriceGroupManager(IProductPriceGroupRepository repository)
	: IProductPriceGroupManager
{
	public void Delete(LSCoreIdRequest request) => repository.HardDelete(request.Id);

	public List<ProductPriceGroupGetDto> GetMultiple() =>
		repository.GetMultiple().ToMappedList<ProductPriceGroupEntity, ProductPriceGroupGetDto>();

	public long Save(ProductPriceGroupSaveRequest request)
	{
		var entity =
			request.Id == 0 ? new ProductPriceGroupEntity() : repository.Get(request.Id!.Value);
		entity.InjectFrom(request);
		repository.UpdateOrInsert(entity);
		return entity.Id;
	}
}
