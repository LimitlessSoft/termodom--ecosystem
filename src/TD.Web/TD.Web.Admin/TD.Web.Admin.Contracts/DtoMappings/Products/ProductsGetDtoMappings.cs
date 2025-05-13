using LSCore.Mapper.Contracts;
using Omu.ValueInjecter;
using TD.Web.Admin.Contracts.Dtos.Products;
using TD.Web.Common.Contracts.Entities;

namespace TD.Web.Admin.Contracts.DtoMappings.Products;

public class ProductsGetDtoMappings : ILSCoreMapper<ProductEntity, ProductsGetDto>
{
	public ProductsGetDto ToMapped(ProductEntity sender)
	{
		var dto = new ProductsGetDto();
		dto.InjectFrom(sender);

		if (sender.Groups != null) // TODO: Check if we should allow this
			dto.Groups = sender.Groups.Select(z => z.Id).ToList();

		if (sender.Price != null) // TODO: Check if we should allow this
		{
			dto.MinWebBase = sender.Price.Min;
			dto.MaxWebBase = sender.Price.Max;
		}

		dto.StockType = sender.StockType;
		dto.UnitId = sender.Unit.Id;
		dto.Classification = (int)sender.Classification;
		return dto;
	}
}
