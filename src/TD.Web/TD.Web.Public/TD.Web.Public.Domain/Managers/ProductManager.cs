using LSCore.Contracts.Http;
using LSCore.Domain.Extensions;
using LSCore.Domain.Managers;
using Microsoft.Extensions.Logging;
using TD.Web.Common.Contracts.Entities;
using TD.Web.Common.Contracts.Requests.Images;
using TD.Web.Common.Repository;
using TD.Web.Public.Contrats.Dtos.Products;
using TD.Web.Public.Contrats.Interfaces.IManagers;
using TD.Web.Public.Contrats.Requests.Products;

namespace TD.Web.Public.Domain.Managers
{
    public class ProductManager : LSCoreBaseManager<ProductManager, ProductEntity>, IProductManager
    {
        public ProductManager(ILogger<ProductManager> logger, WebDbContext dbContext)
            : base(logger, dbContext)
        {
        }

        public LSCoreFileResponse GetImageForProduct(ProductsGetImageRequest request)
        {
            var response = new LSCoreFileResponse();

            var query = First(x => x.IsActive && x.Src.Equals(request.Src));
            response.Merge(query);
            if (response.NotOk)
                return response;

            var ImageGetRequest = new ImagesGetRequest();

            return response;
        }

        public LSCoreListResponse<ProductsGetDto> GetMultiple(ProductsGetRequest request) =>
            new LSCoreListResponse<ProductsGetDto>(
                Queryable(x => x.IsActive)
                .ToDtoList<ProductsGetDto, ProductEntity>());
    }
}
