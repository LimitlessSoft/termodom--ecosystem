using LSCore.Mapper.Contracts;
using TD.Web.Common.Contracts.Entities;
using TD.Web.Common.Contracts.Enums;
using TD.Web.Public.Contracts.Dtos.Products;

namespace TD.Web.Public.Contracts.DtoMappings.Products;

public class ProductsGetDtoMappings : ILSCoreMapper<ProductEntity, ProductsGetDto>
{
	public ProductsGetDto ToMapped(ProductEntity sender) =>
		new()
		{
			Id = sender.Id,
			Title = sender.Name,
			VAT = sender.VAT,
			Unit = sender.Unit.Name,
			PriorityIndex = sender.PriorityIndex,
			Src = sender.Src,
			Classification = sender.Classification,
			IsWholesale = sender.Groups.Any(x => x.Type == ProductGroupType.Veleprodaja),
			StockType = sender.StockType
		};
}
