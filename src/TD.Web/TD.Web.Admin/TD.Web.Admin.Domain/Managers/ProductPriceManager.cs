using LSCore.Common.Contracts;
using LSCore.Mapper.Domain;
using Omu.ValueInjecter;
using TD.Web.Admin.Contracts.Dtos.ProductPrices;
using TD.Web.Admin.Contracts.Interfaces.IManagers;
using TD.Web.Admin.Contracts.Requests.ProductsPrices;
using TD.Web.Common.Contracts.Entities;
using TD.Web.Common.Contracts.Interfaces.IRepositories;

namespace TD.Web.Admin.Domain.Managers;

public class ProductPriceManager(IProductPriceRepository repository) : IProductPriceManager
{
	public List<ProductsPricesGetDto> GetMultiple() =>
		repository.GetMultiple().ToMappedList<ProductPriceEntity, ProductsPricesGetDto>();

	public void Delete(LSCoreIdRequest request) => repository.HardDelete(request.Id);

	public long Save(SaveProductPriceRequest request)
	{
		var entity = request.Id == 0 ? new ProductPriceEntity() : repository.Get(request.Id!.Value);
		entity.InjectFrom(request);
		repository.UpdateOrInsert(entity);
		return entity.Id;
	}
}
