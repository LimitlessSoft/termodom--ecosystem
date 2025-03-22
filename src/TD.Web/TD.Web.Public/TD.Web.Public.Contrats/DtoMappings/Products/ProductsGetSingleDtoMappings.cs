using LSCore.Mapper.Contracts;
using TD.Web.Common.Contracts.Entities;
using TD.Web.Common.Contracts.Enums;
using TD.Web.Public.Contracts.Dtos.Products;

namespace TD.Web.Public.Contracts.DtoMappings.Products;

public class ProductsGetSingleDtoMappings : ILSCoreMapper<ProductEntity, ProductsGetSingleDto>
{
	public ProductsGetSingleDto ToMapped(ProductEntity sender) =>
		new()
		{
			Id = sender.Id,
			FullDescription = sender.Description,
			OneAlternatePackageEquals = sender.OneAlternatePackageEquals,
			Unit = sender.Unit.Name,
			AlternateUnit = sender.AlternateUnit?.Name,
			Title = sender.Name,
			Vat = sender.VAT,
			CatalogId = sender.CatalogId,
			Classification = sender.Classification,
			ShortDescription = sender.ShortDescription,
			MetaDescription = sender.MetaDescription,
			IsWholesale = sender.Groups.Any(x => x.Type == ProductGroupType.Veleprodaja),
			MetaTitle = sender.MetaTitle,
			StockType = sender.StockType
		};
}
