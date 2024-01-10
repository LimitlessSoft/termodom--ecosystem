using LSCore.Contracts.Http;

namespace TD.Office.Public.Contracts.Interfaces.IManagers
{
    public interface ITDWebAdminApiManager
    {
        Task<LSCoreResponse<List<Web.Admin.Contracts.Dtos.Products.ProductsGetDto>>> ProductsGetMultipleAsync();
        Task<LSCoreResponse<List<Web.Admin.Contracts.Dtos.KomercijalnoWebProductLinks.KomercijalnoWebProductLinksGetDto>>> KomercijalnoKomercijalnoWebProductsLinksGetMultipleAsync();
    }
}
