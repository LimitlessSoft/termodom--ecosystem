using LSCore.Contracts.Http;
using LSCore.Domain.Extensions;
using LSCore.Domain.Managers;
using Microsoft.Extensions.Logging;
using TD.Web.Common.Contracts;
using TD.Web.Common.Contracts.Entities;
using TD.Web.Common.Contracts.Interfaces.IManagers;
using TD.Web.Common.Contracts.Requests.Images;
using TD.Web.Common.Domain.Managers;
using TD.Web.Common.Repository;
using TD.Web.Public.Contrats.Dtos.Products;
using TD.Web.Public.Contrats.Interfaces.IManagers;
using TD.Web.Public.Contrats.Requests.Products;

namespace TD.Web.Public.Domain.Managers
{
    public class ProductManager : LSCoreBaseManager<ProductManager, ProductEntity>, IProductManager
    {
        private IImageManager _imageManager;
        public ProductManager(IImageManager imageManager,ILogger<ProductManager> logger, WebDbContext dbContext)
            : base(logger, dbContext)
        {
            _imageManager = imageManager;
        }

        public async Task<LSCoreFileResponse> GetImageForProductAsync(string request)
        {
            var response = new LSCoreFileResponse();
            var query = First(x => x.IsActive && x.Src.Equals(request));
            response.Merge(query);
            if (response.NotOk)
                return response;

            var imageGetRequest = new ImagesGetRequest();
            imageGetRequest.Image = query.Payload.Image;
            imageGetRequest.Quality = Constants.DefaultImageQuality;

            return await _imageManager.GetImageAsync(imageGetRequest);
        }

        public LSCoreListResponse<ProductsGetDto> GetMultiple(ProductsGetRequest request) =>
            new LSCoreListResponse<ProductsGetDto>(
                Queryable(x => x.IsActive)
                .ToDtoList<ProductsGetDto, ProductEntity>());
    }
}
