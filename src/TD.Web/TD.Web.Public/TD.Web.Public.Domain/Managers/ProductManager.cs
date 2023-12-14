using LSCore.Contracts.Extensions;
using LSCore.Contracts.Http;
using LSCore.Domain.Extensions;
using LSCore.Domain.Managers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TD.Web.Common.Contracts;
using TD.Web.Common.Contracts.Entities;
using TD.Web.Common.Contracts.Interfaces.IManagers;
using TD.Web.Common.Contracts.Requests.Images;
using TD.Web.Common.Repository;
using TD.Web.Public.Contracts.Dtos.Products;
using TD.Web.Public.Contrats.Dtos.Products;
using TD.Web.Public.Contrats.Interfaces.IManagers;
using TD.Web.Public.Contrats.Requests.Products;

namespace TD.Web.Public.Domain.Managers
{
    public class ProductManager : LSCoreBaseManager<ProductManager, ProductEntity>, IProductManager
    {
        private readonly IImageManager _imageManager;

        public ProductManager(ILogger<ProductManager> logger, WebDbContext dbContext,
            IImageManager imageManager)
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

            return await _imageManager.GetImageAsync(new ImagesGetRequest() {
                Image = query.Payload.Image,
                Quality = Constants.DefaultImageQuality,
            });
        }

        public LSCoreListResponse<ProductsGetDto> GetMultiple(ProductsGetRequest request) =>
            new LSCoreListResponse<ProductsGetDto>(
                Queryable(x => x.IsActive)
                .ToDtoList<ProductsGetDto, ProductEntity>());

        public LSCoreResponse<ProductsGetSingleDto> GetSingle(string src)
        {
            var response = new LSCoreResponse<ProductsGetSingleDto>();

            var product =
                Queryable(x =>
                    x.IsActive &&
                    x.Src == src)
                .Include(x => x.Unit)
                .Include(x => x.Groups)
                .FirstOrDefault();

            if(product == null)
                return LSCoreResponse<ProductsGetSingleDto>.NotFound();

            response.Payload = product.ToDto<ProductsGetSingleDto, ProductEntity>();

            var imageResponse = _imageManager.GetImageAsync(new Common.Contracts.Requests.Images.ImagesGetRequest()
            {
                Image = product.Image,
                Quality = 1024
            }).GetAwaiter().GetResult();
            response.Merge(imageResponse);
            if (response.NotOk)
                return response;

            response.Payload.ImageData = imageResponse.Payload;
            return response;
        }
    }
}
