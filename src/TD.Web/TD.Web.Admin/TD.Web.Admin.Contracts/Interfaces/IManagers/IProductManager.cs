using LSCore.Common.Contracts;
using TD.Web.Admin.Contracts.Dtos.Products;
using TD.Web.Admin.Contracts.Requests.Products;
using TD.Web.Common.Contracts.Dtos;
using TD.Web.Common.Contracts.Entities;

namespace TD.Web.Admin.Contracts.Interfaces.IManagers;

public interface IProductManager
{
	ProductsGetDto Get(LSCoreIdRequest request);
	ProductsGetDto? GetBySlug(string slug);
	List<ProductsGetDto> GetMultiple(ProductsGetMultipleRequest request);
	List<ProductsGetDto> GetSearch(ProductsGetSearchRequest request);
	long Save(ProductsSaveRequest request);
	List<IdNamePairDto> GetClassifications();
	void UpdateMaxWebOsnove(ProductsUpdateMaxWebOsnoveRequest request);
	void UpdateMinWebOsnove(ProductsUpdateMinWebOsnoveRequest request);
	bool HasPermissionToEdit(long productId);
	bool HasPermissionToEdit(IQueryable<ProductEntity> products, long productId);
	void AppendSearchKeywords(CreateProductSearchKeywordRequest request);
	void DeleteSearchKeywords(DeleteProductSearchKeywordRequest request);
    ProductsGetLinkedDto GetLinked(LSCoreIdRequest idRequest);
    void SetLink(LSCoreIdRequest idRequest, string link);
}
